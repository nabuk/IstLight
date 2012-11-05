﻿
namespace IstLight
{
    public class TickerDescription
    {
        public string Name { get; set; }
        public bool CanBeBought { get; set; }
    }

    public static class TickerExtensions
    {
        public static TickerDescription GetDescription(this Ticker ticker)
        {
            return new TickerDescription { CanBeBought = ticker.CanBeBought, Name = ticker.Name };
        }
    }
}