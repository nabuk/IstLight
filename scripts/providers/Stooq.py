import System
from System import ArgumentException
import clr
clr.AddReference("System.Core")
clr.AddReference("IstLight.Domain");
from IstLight.Services import *
from System.Collections.Generic import *
from System.Net import *
from System import DateTime,Double,String
from System.Globalization import NumberStyles,CultureInfo
from System.Linq import Enumerable
clr.ImportExtensions(System.Linq)

providerSiteUrl = "http://stooq.com"
def ProviderGetTickerDataUrl(ticker): return "http://stooq.com/q/d/l/?s="+ticker+"&i=d"
def ProviderGetTickersUrl(ticker): return "http://stooq.com/cmp/?q=" + ticker

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
		result = client.DownloadString(fileUrl)
	if (result is None) or (result == ''):
		raise ApplicationException('Can\'t download data')
	elif result.Length < 10:
		raise ApplicationException('No data')
	return result

#Download ticker
def Get(ticker):
    rawData = GetRawData(providerSiteUrl, ProviderGetTickerDataUrl(ticker))
    rawRows = rawData.Replace(oldValue = '\r', newValue = '').Split('\n').Skip(1).TakeWhile(lambda row: row.Length > 6).Select(lambda row: row.Split(',')[:2])
    result = rawRows.Select(lambda row: (DateTime.Parse(row[0],CultureInfo.InvariantCulture),Double.Parse(row[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture))).ToList()
    return result

#Find available tickers
def Search(hint):
    rawData = GetRawData(providerSiteUrl, ProviderGetTickersUrl(hint))
    rawData = rawData.Substring(14,rawData.Length - 17).Replace(oldValue = "<b>", newValue = "").Replace(oldValue = "</b>", newValue = "")
    rawData = rawData.Split("|").Select(lambda x: x.Split('~'))
    rawData = rawData.Select(lambda x: (x[0], x[1] if x.Length > 1 else String.Empty, x[2] if x.Length > 2 else None)).ToList()
    if(rawData.Count == 1 and rawData[0][0].Length == 0): rawData.Clear()
    return rawData.Select(lambda x: TickerSearchResult(x[0], x[1] + (String.Empty if String.IsNullOrWhiteSpace(x[2]) else ", Market: "+x[2]))).ToArray();