using System;

namespace IstLight
{
    public class TickerQuote : ITickerQuote, IDate
    {
        public TickerQuote(DateTime date, double value, double? volume) : this(date,value,value,value,value,volume) { }

        public TickerQuote(DateTime date, double open, double close, double high, double low, double? volume)
        {
            if (volume == 0) volume = null;
            if (volume < 0) throw new ArgumentException("Cannot be negative.", "volume");
            if (high < Math.Max(open, close)) throw new ArgumentException("Value is not valid.", "high");
            if (low > Math.Min(open, close)) throw new ArgumentException("Value is not valid.", "low");

            this.Date = date;
            this.Open = open;
            this.Close = close;
            this.High = high;
            this.Low = low;
            this.Volume = volume;
        }

        public DateTime Date { get; private set; }

        public double Open { get; private set; }
        public double Close { get; private set; }
        public double High { get; private set; }
        public double Low { get; private set; }
        public double? Volume { get; private set; }
    }
}
