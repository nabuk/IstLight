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
