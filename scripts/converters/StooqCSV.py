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

import System
from System import ArgumentException
import clr
clr.AddReference("System.Core")
clr.AddReference("IstLight.Domain");
from IstLight.Services import *
from IstLight import *
from System.Collections.Generic import *
from System import DateTime,Double,String, Array
from System.Globalization import NumberStyles,CultureInfo,DateTimeStyles
from System.Text import Encoding
from System.Linq import Enumerable
clr.ImportExtensions(System.Linq)

Name = "Stooq.com"
Format = "CSV;TXT"

def ExtractDate(x):
	return DateTime.ParseExact(x,Array[String](("yyyy-MM-dd","yyyyMMdd")),CultureInfo.InvariantCulture,DateTimeStyles.AssumeLocal)
def ExtractDouble(x): return Double.Parse(x, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture)
def ExtractQuote(row):
	date = ExtractDate(row[0])
	open = ExtractDouble(row[1])
	high = ExtractDouble(row[2])
	low = ExtractDouble(row[3])
	close = ExtractDouble(row[4])
	volume = ExtractDouble(row[5]) if row.Length > 5 else None
	return TickerQuote(date,open,close,high,low,volume)

def Read(rawTicker):
	rawData = Encoding.Default.GetString(rawTicker.Data)
	rawRows = rawData.Replace(oldValue = '\r', newValue = '').Split('\n').Skip(1).TakeWhile(lambda row: row.Length > 6).Select(lambda row: row.Split(','))
	quotes = Array[TickerQuote](rawRows.Select(lambda row: ExtractQuote(row)).ToArray())
	quotes = IReadOnlyListExtensions.AsReadOnlyList(quotes)
	return Ticker(rawTicker.Name,quotes)
	
def Save(ticker):
	raise NotImplementedException()
