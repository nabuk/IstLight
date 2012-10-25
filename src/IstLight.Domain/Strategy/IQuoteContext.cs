using System;
using IstLight.Domain.Synchronization;

namespace IstLight.Domain.Strategy
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
