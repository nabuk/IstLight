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
from IstLight.Simulation import *
from System.Linq import Enumerable
clr.ImportExtensions(System.Linq)

Category = 'Averaged exposition'

def AsStringPair(k,v): return KeyValuePair[String,String](k,v)
def InvariantFormat(format,x): return String.Format(CultureInfo.InvariantCulture,format, x)

def GetEquitiesAt(result, bar):
	l = []
	for tickerI in range(result.Descriptions.Count):
		l.append(SimulationResultQuoteExtensions.TickerEquity(result[bar], tickerI, result.SyncTickers))
	l.append(result[bar].Cash)
	return l

def GetEquityMatrix(result):
	matrix = []
	for barI in range(result.Count):
		matrix.append(GetEquitiesAt(result,barI))
	return matrix

def ComputeExpositionsAtBar(expList):
	s = sum(expList)
	for i in range(expList.Count):
		expList[i] = expList[i] / s
	return expList

def GetExpositions(result):
	l = GetEquityMatrix(result)
	for i,x in enumerate(l):
		l[i] = ComputeExpositionsAtBar(x)
	exps = []
	for i in range(result.Descriptions.Count+1):
		exps.append(sum([x[i] for x in l]) / result.Count)
	return exps
		
def DescribeExpositions(result, exps):
	descCount = result.Descriptions.Count
	descs = list(result.Descriptions)
	for i in range(descCount):
		exps[i] = (descs[i].Name, exps[i])
	exps[descCount] = ("Cash", exps[descCount])
	exps = [exps[descCount]] + [r for r in sorted(exps[:descCount], cmp = lambda x,y: y[1] > x[1]) if r[1] > 0]
	return Array[KeyValuePair[String,String]]([AsStringPair(x[0], InvariantFormat('{0:0.00} %', x[1] * 100)) for x in exps])
		
def Analyze(result):
	return DescribeExpositions(result, GetExpositions(result))