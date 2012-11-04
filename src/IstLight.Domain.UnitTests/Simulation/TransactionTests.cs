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
        public void ctor_NetProfit_IsSet(Transaction sut)
        {
            Assert.Equal<double>(1, sut.NetProfit);
        }

        public static IEnumerable<object[]> Sut
        {
            get
            {
                yield return new object[] { new Transaction(TransactionType.Sell, 1, 1, 1) };
            }
        }
    }
}
