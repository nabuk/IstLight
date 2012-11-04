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
