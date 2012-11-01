using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.UnitTests.Synchronization
{
    public class ValidCanSynchronizeTestCaseProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return
                new ValidSynchronizationTestCaseProvider()
                .Select(x => new object[] { x[0], x[1], x[3] })
                .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
