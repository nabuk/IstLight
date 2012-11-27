import clr
import System
clr.AddReference("System.Core")
clr.AddReference("IstLight.Domain")
from System import *
from System.Globalization import CultureInfo
from System.Collections.Generic import *
from IstLight.Simulation import *

Category = 'Averaged exposition'

def AsStringPair(k,v): return KeyValuePair[String,String](k,v)
def InvariantFormat(format,x): return String.Format(CultureInfo.InvariantCulture,format, x)

def Equity(result, bar): return SimulationResultQuoteExtensions.Equity(result[bar], result.SyncTickers)
def Cash(result, bar): return result[bar].Cash
def ExpositionToTicker(result, bar, tickerIndex): return SimulationResultQuoteExtensions.ExpositionToTicker(result[bar], tickerIndex, result.SyncTickers)

def YieldCashExp(result):
	for i,x in enumerate(result):
		yield x.Cash / Equity(result,i)
def YieldTickerExp(result,tickerIndex):
	for i,x in enumerate(result):
		yield ExpositionToTicker(result,i,tickerIndex)
def Average(iterable):
	l = list(iterable)
	return sum(l) / len(l)

def GetCashExpList(result):
	return [AsStringPair('Cash', InvariantFormat('{0:0.00} %', Average(YieldCashExp(result))*100))]
def GetTickersExpList(result):
	l = []
	for i,x in enumerate(result.Descriptions):
		l.append(AsStringPair(x.Name, InvariantFormat('{0:0.00} %', Average(YieldTickerExp(result,i))*100)))
	return l
	
def Analyze(result):
	return Array[KeyValuePair[String,String]]( GetCashExpList(result) + GetTickersExpList(result) )