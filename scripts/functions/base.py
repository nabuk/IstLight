def getDate():
	return __quotes__.Date

def getIndex(ticker):
	tIndex = None
	if type(ticker) is str: tIndex = __quotes__.GetTickerIndex(ticker)
	else: tIndex = int(ticker)
	return tIndex

def isFirst():
	return __quotes__.IsFirst
	
def isLast():
	return __quotes__.IsLast
	
def getQuotes(ticker):
	tIndex = getIndex(ticker)
	return __quotes__.GetQuotes(tIndex)

def getPrice(ticker,default = None):
	qs = getQuotes(ticker)
	if qs.Count == 0: return default
	else: return qs[0].Value
	
def getPrices(ticker):
	for x in getQuotes(ticker):
		yield x.Value
		
def getVolume(ticker,default = None):
	qs = getQuotes(ticker)
	if qs.Count == 0: return default
	else: return qs[0].Volume
	
def getVolumes(ticker):
	for x in getQuotes(ticker):
		yield x.Volume
	
def getQuantity(ticker):
	tIndex = getIndex(ticker)
	return __wallet__.GetQuantity(tIndex)
	
def setQuantity(ticker, newQuantity):
	tIndex = getIndex(ticker)
	return __wallet__.SetQuantity(tIndex, newQuantity)

def getCash():
	return __wallet__.Cash
	
def getEquity(ticker):
	tIndex = getIndex(ticker)
	return getPrice(tIndex,0) * getQuantity(tIndex)
	
def getWalletEquity():
	walletEquity = getCash()
	for tI in range(__quotes__.TickerCount):
		walletEquity += getEquity(tI)
	return walletEquity
	
def getShare(ticker):
	return getEquity(ticker) / getWalletEquity()

def setShare(ticker, share):
	tIndex = getIndex(ticker)
	walletEquity = getWalletEquity()
	newEquity = share*walletEquity
	price = getPrice(tIndex)
	return (False if price == None else setQuantity(tIndex, newEquity / price))
	
def getCashShare():
	return getCash() / getWalletEquity()