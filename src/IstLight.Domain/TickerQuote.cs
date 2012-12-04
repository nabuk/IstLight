// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

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
