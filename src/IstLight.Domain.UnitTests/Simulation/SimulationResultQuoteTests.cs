using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Simulation;
using IstLight.Domain.Extensions;
using Xunit.Extensions;
using IstLight.Domain.Synchronization;
using Xunit;
using Moq;
using System.Collections;

namespace IstLight.Domain.UnitTests.Simulation
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
            var transactions = new Transaction[] { new Transaction(TransactionType.Buy, 1, 1, 0) }.AsReadOnlyList();
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
