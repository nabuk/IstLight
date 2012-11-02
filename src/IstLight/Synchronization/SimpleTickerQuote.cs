using System;

namespace IstLight.Synchronization
{
    public class SimpleTickerQuote : ISimpleTickerQuote
    {
        public SimpleTickerQuote(DateTime date, double value, double? volume)
        {
            if (volume <= 0) throw new ArgumentException("Cannot be negative or zero.", "volume");
            this.Value = value;
            this.Volume = volume;
            this.Date = date;
        }
        
        public double Value
        {
            get;
            private set;
        }
        public double? Volume
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get;
            private set;
        }
    }
}
