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

Category = 'Net profit (by year)'

def AsStringPair(k,v): return KeyValuePair[String,String](k,v)
def InvariantFormat(format,x): return String.Format(CultureInfo.InvariantCulture,format, x)
def Equity(result, bar): return SimulationResultQuoteExtensions.Equity(result[bar], result.SyncTickers)
def GetEquityArray(result): return [(i,result[i].Date,Equity(result,i)) for i in range(result.Count)]
def Analyze(result):
	equities = GetEquityArray(result)
	endOfYear = []
	equities = [(-1,DateTime(equities[0][1].Year-1,12,31),result.Settings.Get[InitialEquitySetting]().Value)] + equities
	count = len(equities)
	result = []
	for i in range(count):
		if i+1 < count:
			if equities[i][1].Year != equities[i+1][1].Year:
				endOfYear.append(equities[i])
		elif equities[i][1].Year != endOfYear[ len(endOfYear)-1 ][1].Year:
			endOfYear.append(equities[i])
	for i in range(1,len(endOfYear)):
		profit = ((endOfYear[i][2]/endOfYear[i-1][2])-1)*100
		year = endOfYear[i][1].Year
		result.append(AsStringPair(str(year), InvariantFormat('{0:0.00} %', profit)))
	return Array[KeyValuePair[String,String]](result)
			
	
