using System;
using System.Collections.Generic;

using IstLight.Settings;

namespace IstLight.UnitTests.Synchronization
{
    public class TickerQuoteWithPriceProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (SimulationPriceType priceType in Enum.GetValues(typeof(SimulationPriceType)))
                foreach (double? volume in new double?[] { 1, null })
                    foreach (double low in new double[] { -1, 0 })
                        foreach (double high in new double[] { 0, 1 })
                            yield return new object[] { new TickerQuote(new DateTime(2000, 1, 1), high, low, high, low, volume), priceType };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
