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
