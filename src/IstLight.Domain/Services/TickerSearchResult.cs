﻿// Copyright 2012 Jakub Niemyjski
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

namespace IstLight.Services
{
    public class TickerSearchResult
    {
        public TickerSearchResult(string name, string description, string market)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("tickerName", "tickerName is null or empty.");

            Name = name;
            Description = description ?? "";
            Market = market ?? "";
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Market { get; private set; }
    }
}
