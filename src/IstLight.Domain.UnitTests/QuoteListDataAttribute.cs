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
using Moq;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class QuoteListDataAttribute : DataAttribute
    {
        private readonly int[] dayOffsets;

        public QuoteListDataAttribute(params int[] dayOffsets)
        {
            this.dayOffsets = dayOffsets;
        }

        public QuoteList<IDate> CreateCollection()
        {
            return new QuoteList<IDate>(CreateIDateIEnumerable().AsReadOnlyList());
        }

        public override IEnumerable<object[]> GetData(System.Reflection.MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            yield return new object[] { CreateCollection() };
        }

        private IEnumerable<IDate> CreateIDateIEnumerable()
        {
            DateTime start = new DateTime(2000, 1, 1);
            for (int i = 0; i < dayOffsets.Length; i++)
            {
                var mock = new Mock<IDate>();
                mock.Setup(x => x.Date).Returns(start.AddDays(dayOffsets[i]));
                yield return mock.Object;
            }
        }
    }
}
