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

using System.Collections.Generic;
using IstLight.Simulation;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Simulation
{
    public class TransactionTests
    {
        [Theory, PropertyData("Sut")]
        public void ctor_Type_IsSet(Transaction sut)
        {
            Assert.Equal<TransactionType>(TransactionType.Sell, sut.Type);
        }

        [Theory, PropertyData("Sut")]
        public void ctor_TickerIndex_IsSet(Transaction sut)
        {
            Assert.Equal<int>(1, sut.TickerIndex);
        }

        [Theory, PropertyData("Sut")]
        public void ctor_Quantity_IsSet(Transaction sut)
        {
            Assert.Equal<double>(1, sut.Quantity);
        }

        [Theory, PropertyData("Sut")]
        public void ctor_NetProfitRate_IsSet(Transaction sut)
        {
            Assert.Equal<double>(1, sut.NetProfitRate);
        }

        public static IEnumerable<object[]> Sut
        {
            get
            {
                yield return new object[] { new Transaction(TransactionType.Sell, 1, 1, 1, 0, 1) };
            }
        }
    }
}
