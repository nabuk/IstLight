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
using IstLight.Simulation;
using IstLight.Synchronization;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Simulation
{
    public class SimulationResultQuoteTests
    {
        [Theory, PropertyData("Sut")]
        public void ctor_TickerQuantity_IsSet(SimulationResultQuote sut)
        {
            Assert.True(TickerQuantity.SequenceEqual(sut.TickerQuantity), "TickerQuantity is wrong.");
        }

        [Theory, PropertyData("Sut")]
        public void ctor_Cash_IsSet(SimulationResultQuote sut)
        {
            Assert.Equal<double>(1, sut.Cash);
        }

        [Theory, PropertyData("Sut")]
        public void ctor_Observation_IsSet(SimulationResultQuote sut)
        {
            var comparer = new LambdaEqualityComparer<Observation>(
                EqualsMethod: (o1,o2) => o1.Date == o2.Date && o1.CurrentQuoteCount.SequenceEqual(o2.CurrentQuoteCount));

            Assert.Equal<Observation>(Observation, sut.Observation, comparer );
        }

        [Theory, PropertyData("Sut")]
        public void Date_IsEqualToObservationDate(SimulationResultQuote sut)
        {
            Assert.Equal<DateTime>(Observation.Date, sut.Date);
        }

        [Theory, PropertyData("Sut")]
        public void Transactions_SetToNull_GetReturnsEmptyCollection(SimulationResultQuote sut)
        {
            sut.Transactions = null;
            Assert.Equal<int>(0, sut.Transactions.Count);
        }

        [Theory, PropertyData("Sut")]
        public void Transactions_SetToSth_GetReturnsWhatWasSet(SimulationResultQuote sut)
        {
            var transactions = new Transaction[] { new Transaction(TransactionType.Buy, 1, 1, 0, 0, 1) }.AsReadOnlyList();
            sut.Transactions = transactions;
            Assert.Same(transactions, sut.Transactions);
        }

        public static SimulationResultQuote CreateSut()
        {
            return new SimulationResultQuote(TickerQuantity, 1, Observation);
        }
        public static IEnumerable<object[]> Sut
        {
            get
            {
                yield return new object[] { CreateSut() };
            }
        }

        private static Observation Observation
        {
            get { return new Observation(new int[] { 1 }.AsReadOnlyList(), new DateTime(2000, 1, 1)); }
        }
        private static IReadOnlyList<double> TickerQuantity
        {
            get { return new double[] { 1 }.AsReadOnlyList(); }
        }
    }
}
