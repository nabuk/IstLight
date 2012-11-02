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
