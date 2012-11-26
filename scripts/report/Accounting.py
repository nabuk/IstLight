import clr
import System
clr.AddReference("System.Core")
clr.AddReference("IstLight.Domain")
from System import *
from System.Globalization import CultureInfo
from System.Collections.Generic import *
from math import pow
from IstLight.Simulation import *
from IstLight.Settings import *
clr.ImportExtensions(System.Linq)

Category = 'Accounting'

def AsStringPair(k,v): return KeyValuePair[String,String](k,v)
def YearDiff(result): return (result.To - result.From).TotalDays / 365.25
def AnnualInflation(result): return result.Settings.Get[AnnualInflationRateSetting]().Value
def TotalInflation(result): return pow(AnnualInflation(result) + 1, YearDiff(result)) - 1
def Equity(result, bar): return SimulationResultQuoteExtensions.Equity(result[bar], result.SyncTickers)
def InflationAdjustment(x, inflation): return (1 / (inflation + 1))*x
def InvariantFormat(format,x): return String.Format(CultureInfo.InvariantCulture,format, x)

def GetInitialEquity(result):
	return AsStringPair('Initial equity', InvariantFormat('${0:0.00}', Equity(result,0)))
def GetTransactionIncome(result):
	income = sum(result.Select(lambda x: sum(x.Transactions.Select(lambda t: t.CashFlow).Where(lambda p: p > 0))))
	return AsStringPair('Transaction income', InvariantFormat('${0:0.00}', income))
def GetTransactionOutcome(result):
	outcome = sum(result.Select(lambda x: sum(x.Transactions.Select(lambda t: t.CashFlow).Where(lambda p: p < 0))))
	return AsStringPair('Transaction outcome', InvariantFormat('${0:0.00}', outcome))
def GetTradeProfit(result):
	profit = sum(result.Select(lambda x: sum(x.Transactions.Select(lambda t: t.NetProfit).Where(lambda p: p > 0))))
	return AsStringPair('Trade profit', InvariantFormat('${0:0.00}', profit))
def GetTradeLoss(result):
	loss = sum(result.Select(lambda x: sum(x.Transactions.Select(lambda t: t.NetProfit).Where(lambda p: p < 0))))
	return AsStringPair('Trade loss', InvariantFormat('${0:0.00}', loss))
def GetCommissions(result):
	commissions = sum(result.Select(lambda x: sum(x.Transactions.Select(lambda t: t.Commission))))
	return AsStringPair('Commissions', InvariantFormat('${0:0.00}', commissions))
def GetInterestCredited(result):
	interestSum = sum(result.Select(lambda x: x.Interest))
	return AsStringPair('Interest credited', InvariantFormat('${0:0.00}', interestSum))
def GetOpenPositions(result):
	return AsStringPair('Open positions', InvariantFormat('${0:0.00}', Equity(result,result.Count-1) - result[result.Count-1].Cash))
def GetFinalEquity(result):
	return AsStringPair('Final equity', InvariantFormat('${0:0.00}', Equity(result,result.Count-1)))
def GetAdjustedFinalEquity(result):
	return AsStringPair('Final equity (adjusted)', InvariantFormat('${0:0.00}', InflationAdjustment(Equity(result,result.Count-1), TotalInflation(result))))

def Analyze(result):
	return Array[KeyValuePair[String,String]]([
		GetInitialEquity(result),
		GetFinalEquity(result),
		GetAdjustedFinalEquity(result),
		GetTransactionIncome(result),
		GetTransactionOutcome(result),
		GetTradeProfit(result),
		GetTradeLoss(result),
		GetCommissions(result),
		GetInterestCredited(result),
		GetOpenPositions(result)])