# Copyright 2012 Jakub Niemyjski
#
# This file is part of IstLight.
#
# IstLight is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# IstLight is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

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

Category = 'Riskiness'

def AsStringPair(k,v): return KeyValuePair[String,String](k,v)
def YearDiff(result): return (result.To - result.From).TotalDays / 365.25
def AnnualInflation(result): return result.Settings.Get[AnnualInflationRateSetting]().Value
def AnnualInterest(result): return result.Settings.Get[AnnualInterestRateSetting]().Value
def TotalInflation(result): return pow(AnnualInflation(result) + 1, YearDiff(result)) - 1
def Equity(result, bar): return SimulationResultQuoteExtensions.Equity(result[bar], result.SyncTickers)
def InflationAdjustment(x, inflation): return (1 / (inflation + 1))*x
def InvariantFormat(format,x): return String.Format(CultureInfo.InvariantCulture,format, x)
def HighestDrawdown(result, startIndex, endIndex):
	drawDown = 1.0
	bestPeak = 0.0
	for v in range(endIndex - startIndex + 1).Select(lambda i: Equity(result,i)):
		if v > bestPeak:
			bestPeak = v
		elif v / bestPeak < drawDown:
			drawDown = v / bestPeak
	return drawDown - 1
	
def GetHighestDrawdown(result):
	drawDown = HighestDrawdown(result,0,result.Count-1) * 100
	return AsStringPair('Highest drawdown', InvariantFormat('{0:0.00} %', drawDown))
def GetLongestFlatRange(result):
	maxDaySpan = 0
	maxValue = Equity(result,0)
	maxDate = result[0].Date
	for i,x in enumerate(result):
		eq = Equity(result,i)
		if eq > maxValue:
			maxValue = eq
			maxDate = result[i].Date
		else:
			maxDaySpan = max(maxDaySpan, (result[i].Date - maxDate).TotalDays)
	return AsStringPair('Longest flat range', InvariantFormat('{0} day' + ('s' if maxDaySpan != 1 else ''), maxDaySpan))
	
def FindYearAfterIndex(result,startIndex):
	i = startIndex
	while i < result.Count:
		if (result[i].Date - result[startIndex].Date).TotalDays > 365:
			return i
		i+=1
	return -1
def ExtractYearPeriodIndices(result):
	startIndex = 0
	while startIndex >=0:
		yield startIndex
		startIndex = FindYearAfterIndex(result,startIndex)
def ExtractYearlyReturns(result):
	yIndices = list(ExtractYearPeriodIndices(result))
	return yIndices.Zip(yIndices.Skip(1), lambda x,y: (Equity(result,y) / Equity(result,x)) - 1)
def ComputeStandardDeviation(retL):
	if len(retL) > 3:
		avg = sum(retL) / len(retL)
		sd = pow(
				sum(pow(r - avg,2) for r in retL) / len(retL)
				,0.5)
		return sd
	else:
		return None

def GetStandardDeviation(result):
	sd = ComputeStandardDeviation(list(ExtractYearlyReturns(result)))
	return AsStringPair('Standard deviation', 'not enough data' if sd == None else InvariantFormat('{0:0.00} %', sd*100))
def GetSharpeRatio(result):
	retL = list(ExtractYearlyReturns(result))
	ratio = 'not enough data'
	sd = ComputeStandardDeviation(retL)
	if sd == 0: ratio = 'standard deviation must be greater than 0'
	elif sd != None:
		avg = sum(retL) / len(retL)
		interestRate = AnnualInterest(result)
		ratio = (avg - interestRate) / sd
		ratio = InvariantFormat('{0:0.00}', ratio)
	return AsStringPair('Sharpe ratio', ratio)

def GetSterlingRatio(result):
	indices = list(ExtractYearPeriodIndices(result))
	ratio = 'not enough data'
	if len(indices) > 3:
		drops = list(indices.Zip(indices.Skip(1), lambda x,y: abs(HighestDrawdown(result,x,y))).OrderByDescending(lambda x: x).Take(3))
		avgDrop = sum(drops) / len(drops)
		yearDiff = YearDiff(result)
		annualizedPerformance = (pow(Equity(result,result.Count-1) / Equity(result,0), 1 / YearDiff(result)) - 1)
		ratio = annualizedPerformance / (avgDrop + 0.1)
		ratio = InvariantFormat('{0:0.00}', ratio)
	return AsStringPair('Sterling ratio', ratio)

def Analyze(result):
	return Array[KeyValuePair[String,String]]([
		GetHighestDrawdown(result),
		GetLongestFlatRange(result),
		GetStandardDeviation(result),
		GetSharpeRatio(result),
		GetSterlingRatio(result)])