import clr
clr.AddReference("IstLight.Domain")
from System import *
from System.Globalization import CultureInfo
from System.Collections.Generic import *
from math import pow
from IstLight.Simulation import *
from IstLight.Settings import *

Category = 'Performance'

def AsStringPair(k,v): return KeyValuePair[String,String](k,v)
def YearDiff(result): return (result.To - result.From).TotalDays / 365.25
def AnnualInflation(result): return result.Settings.Get[AnnualInflationRateSetting]().Value
def TotalInflation(result): return pow(AnnualInflation(result) + 1, YearDiff(result)) - 1
def Equity(result, bar): return SimulationResultQuoteExtensions.Equity(result[bar], result.SyncTickers)
def InflationAdjustment(x, inflation): return (1 / (inflation + 1))*x
def InvariantFormat(format,x): return String.Format(CultureInfo.InvariantCulture,format, x)

def GetNetProfit(result):
	netProfit = Equity(result,result.Count-1) - Equity(result,0)
	return AsStringPair('Net profit', InvariantFormat('${0:0.00}',netProfit))
def GetAdjustedNetProfit(result):
	netProfitAdj = InflationAdjustment(Equity(result,result.Count-1), TotalInflation(result)) - Equity(result,0)
	return AsStringPair('Net profit (adjusted)',InvariantFormat('${0:0.00}', netProfitAdj))
def GetPerformance(result):
	performance = ((Equity(result,result.Count-1) / Equity(result,0)) - 1)*100
	return AsStringPair('Performance', InvariantFormat('{0:0.00} %', performance))
def GetAdjustedPerformance(result):
	performanceAdj = ((InflationAdjustment(Equity(result,result.Count-1), TotalInflation(result)) / Equity(result,0)) - 1)*100
	return AsStringPair('Performance (adjusted)', InvariantFormat('{0:0.00} %', performanceAdj))
def GetAnnualizedPerformance(result):
	yearDiff = YearDiff(result)
	annualized = 0
	if yearDiff > 0:
		annualized = (pow(Equity(result,result.Count-1) / Equity(result,0), 1 / YearDiff(result)) - 1) * 100
	return AsStringPair('Annualized performance', InvariantFormat('{0:0.00} %', annualized))
def GetAdjustedAnnualizedPerformance(result):
	yearDiff = YearDiff(result)
	annualized = 0
	if yearDiff > 0:
		annualized = (pow(InflationAdjustment(Equity(result,result.Count-1), TotalInflation(result)) / Equity(result,0), 1 / YearDiff(result)) - 1) * 100
	return AsStringPair('Annualized performance (adjusted)', InvariantFormat('{0:0.00} %', annualized))

def Analyze(result):
	return Array[KeyValuePair[String,String]]([
		GetNetProfit(result),
		GetAdjustedNetProfit(result),
		GetPerformance(result),
		GetAdjustedPerformance(result),
		GetAnnualizedPerformance(result),
		GetAdjustedAnnualizedPerformance(result)])