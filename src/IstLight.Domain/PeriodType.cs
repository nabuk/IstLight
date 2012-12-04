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
    public enum PeriodType : int
    {
        Tick,
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month
    }

    public static class PeriodTimespan
    {
        public static TimeSpan ToTimeSpan(this PeriodType period)
        {
            switch (period)
            {
                case PeriodType.Tick: return TimeSpan.FromTicks(1);
                case PeriodType.Second: return TimeSpan.FromSeconds(1);
                case PeriodType.Minute: return TimeSpan.FromMinutes(1);
                case PeriodType.Hour: return TimeSpan.FromHours(1);
                case PeriodType.Day: return TimeSpan.FromDays(1);
                case PeriodType.Week: return TimeSpan.FromDays(7);
                case PeriodType.Month: return TimeSpan.FromDays(30);
                default: throw new NotImplementedException();
            }
        }
    }
}
