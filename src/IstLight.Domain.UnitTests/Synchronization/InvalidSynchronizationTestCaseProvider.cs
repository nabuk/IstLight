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
using System.Collections.Generic;
using System.Linq;
using IstLight.Settings;


namespace IstLight.UnitTests.Synchronization
{
    public class InvalidSynchronizationTestCaseProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return OneTickerMustExist();

            yield return InitialEquityCannotBeSmallerThanOne();

            yield return PercentCommissionMustBeLowerThan100();

            foreach (CommissionType commissionType in Enum.GetValues(typeof(CommissionType)))
                yield return CommissionMustBePossitive(commissionType);

            yield return TickersMustHaveUniqueNames();

            yield return TickersDoNotHaveCommonRange();

            yield return FromAfterTo();

            yield return ExplicitDateRangeHaveNoCommonBarsWithTickers(new DateTime(2000, 1, 1),new DateTime(2001, 1, 1));
            yield return ExplicitDateRangeHaveNoCommonBarsWithTickers(new DateTime(2000, 1, 1), new DateTime(1999, 1, 1));
            yield return ExplicitDateRangeHaveNoCommonBarsWithTickers(new DateTime(2000, 1, 1), new DateTime(1999, 1, 1), new DateTime(2001, 1, 1));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        object[] OneTickerMustExist()
        {
            return new object[]
            {
                new Ticker[0].AsReadOnlyList(),
                new SimulationSettings(),
                "At least one ticker must exist."
            };
        }

        object[] InitialEquityCannotBeSmallerThanOne()
        {
            var settings = new SimulationSettings();
            settings.Get<InitialEquitySetting>().Value = 0.99;
            return new object[]
                {
                    new Ticker[] { TickerTests.CreateTicker() }.AsReadOnlyList(),
                    settings,
                    "Initial equity cannot be smaller than 1."
                };
        }

        object[] PercentCommissionMustBeLowerThan100()
        {
            var settings = new SimulationSettings();
            var commision = settings.Get<CommissionSetting>();
            commision.Type = CommissionType.Percent;
            commision.Value = 100;

            return new object[]
                {
                    new Ticker[] { TickerTests.CreateTicker() }.AsReadOnlyList(),
                    settings,
                    "Percent commission must be lower than 100 %."
                };
        }

        object[] CommissionMustBePossitive(CommissionType commistionType)
        {
            var settings = new SimulationSettings();
            var commision = settings.Get<CommissionSetting>();
            commision.Type = commistionType;
            commision.Value = -0.5;

            return new object[]
                {
                    new Ticker[] { TickerTests.CreateTicker() }.AsReadOnlyList(),
                    settings,
                    "Commission must be possitive."
                };
        }

        object[] TickersMustHaveUniqueNames()
        {
            return new object[]
                {
                    new Ticker[] { TickerTests.CreateTicker("X"),TickerTests.CreateTicker("X") }.AsReadOnlyList(),
                    new SimulationSettings(),
                    "Tickers must have unique names."
                };
        }

        object[] TickersDoNotHaveCommonRange()
        {
            var settings = new SimulationSettings();
            settings.Get<SimulationRangeSetting>().Type = SimulationRangeType.Common;
            return new object[]
            {
                new Ticker[]
                {
                    TickerTests.CreateTickerByOffsets("X", 0,1),
                    TickerTests.CreateTickerByOffsets("Y", 0,1),
                    TickerTests.CreateTickerByOffsets("Z", 2,3),
                    TickerTests.CreateTickerByOffsets("W", 0,1)
                }.AsReadOnlyList(),
                settings,
                "Common range setting requires that all tickers have common range."
            };
        }

        object[] ExplicitDateRangeHaveNoCommonBarsWithTickers(
            DateTime oneDayRangeFrom,
            params DateTime[] oneDayTickerFrom
            )
        {
            var settings = new SimulationSettings();
            var rangeSetting = settings.Get<SimulationRangeSetting>();
            rangeSetting.Type = SimulationRangeType.FromToDates;
            rangeSetting.From = oneDayRangeFrom;
            rangeSetting.To = oneDayRangeFrom.AddDays(1);

            return new object[]
            {
                oneDayTickerFrom.Select(d =>
                new Ticker(Guid.NewGuid().ToString(),
                    Ext.ROL(TickerQuoteTests.CreateByShortCtor(date: d),TickerQuoteTests.CreateByShortCtor(date: d.AddDays(1)))))
                .AsReadOnlyList(),
                settings,
                "Explicit simulation From-To range must match ticker bar dates."
            };
        }

        object[] FromAfterTo()
        {
            var settings = new SimulationSettings();
            var rangeSetting = settings.Get<SimulationRangeSetting>();
            rangeSetting.Type = SimulationRangeType.FromToDates;
            rangeSetting.To = (rangeSetting.From = new DateTime(2000,1,1)).AddDays(-1);

            return new object[]
            {
                new Ticker[] { TickerTests.CreateTicker() }.AsReadOnlyList(),
                settings,
                "Date from cannot be after date to."
            };
        }
    }
}
