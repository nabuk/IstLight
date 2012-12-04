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
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class PeriodTests
    {
        [Theory]
        [ClassData(typeof(PeriodEnumProvider))]
        public void AllPeriods_HaveCorrespondingTimeSpan(PeriodType period)
        {
            Assert.DoesNotThrow(() => period.ToTimeSpan());
        }

        public class PeriodEnumProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (PeriodType period in Enum.GetValues(typeof(PeriodType)))
                    yield return new object[] { period };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
