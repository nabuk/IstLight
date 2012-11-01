using System;
using IstLight.Synchronization;

namespace IstLight.Strategy
{
    public interface IQuoteContext
    {
        DateTime Date { get; }
        int TickerCount { get; }
        IReadOnlyList<ISimpleTickerQuote> GetQuotes(int tickerIndex);
        int? GetTickerIndex(string tickerName);
        TickerDescription GetTickerDescription(int tickerIndex);
        TimeSpan? Span { get; }
        int GetNewQuoteCount(int tickerIndex);
        bool IsLast { get; }
        bool IsFirst { get; }
    }
}
