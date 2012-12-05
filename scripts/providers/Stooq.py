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
from System.Net import *
from System import DateTime,Double,String, Array
from System.Globalization import NumberStyles,CultureInfo
from System.Text import Encoding
from System.Linq import Enumerable
clr.ImportExtensions(System.Linq)

providerSiteUrl = "http://stooq.com"
def ProviderGetTickerDataUrl(ticker): return "http://stooq.com/q/d/l/?s="+ticker+"&i=d"
def ProviderGetTickersUrl(ticker): return "http://stooq.com/cmp/?q=" + ticker
Name = "Stooq.com"

class WebClientEx(WebClient):
	def GetWebRequest(self,address):
		request = WebClient.GetWebRequest(self,address)
		request.CookieContainer = self.CookieContainer
		request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0";
		return request

def GetCookieContainer(url):
	cookieContainer = CookieContainer()
	request = WebRequest.Create(url)
	request.CookieContainer = cookieContainer
	request.Method = "HEAD"
	response = request.GetResponse()
	response.Close()
	return cookieContainer
	
def GetRawData(webUrl, fileUrl):
	result = None
	with WebClientEx() as client:
		client.CookieContainer = GetCookieContainer(webUrl)
		client.Encoding = Encoding.UTF8
		result = client.DownloadString(fileUrl)
	if (result is None) or (result == ''):
		raise ApplicationException('Can\'t download data')
	elif result.Length < 10:
		raise ApplicationException('No data')
	return result

def ExtractDate(x): return DateTime.Parse(x,CultureInfo.InvariantCulture)
def ExtractDouble(x): return Double.Parse(x, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture)
def ExtractQuote(row):
	date = ExtractDate(row[0])
	open = ExtractDouble(row[1])
	high = ExtractDouble(row[2])
	low = ExtractDouble(row[3])
	close = ExtractDouble(row[4])
	volume = ExtractDouble(row[5]) if row.Length > 5 else None
	return TickerQuote(date,open,close,high,low,volume)

#Download ticker
def Get(ticker):
	rawData = GetRawData(providerSiteUrl, ProviderGetTickerDataUrl(ticker))
	rawRows = rawData.Replace(oldValue = '\r', newValue = '').Split('\n').Skip(1).TakeWhile(lambda row: row.Length > 6).Select(lambda row: row.Split(','))
	quotes = Array[TickerQuote](rawRows.Select(lambda row: ExtractQuote(row)).ToArray())
	quotes = IReadOnlyListExtensions.AsReadOnlyList(quotes)
	return Ticker(ticker,quotes)

#Find available tickers
def Search(hint):
	if(String.IsNullOrWhiteSpace(hint)): return Array[TickerSearchResult]([])
	rawData = GetRawData(providerSiteUrl, ProviderGetTickersUrl(hint))
	rawData = rawData.Substring(14,rawData.Length - 17).Replace(oldValue = "<b>", newValue = "").Replace(oldValue = "</b>", newValue = "")
	rawData = rawData.Split("|").Select(lambda x: x.Split('~'))
	rawData = rawData.Select(lambda x: (x[0], x[1] if x.Length > 1 else String.Empty, x[2] if x.Length > 2 else None)).ToList()
	if(rawData.Count == 1 and rawData[0][0].Length == 0): rawData.Clear()
	searchResult = rawData.Select(lambda x: TickerSearchResult(x[0], x[1], x[2]))
	return Array[TickerSearchResult](searchResult.ToArray())