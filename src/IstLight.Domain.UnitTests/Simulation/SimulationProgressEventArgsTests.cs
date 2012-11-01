using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Simulation;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Simulation
{
    public class SimulationProgressEventArgsTests
    {
        [Theory, PropertyData("Sut")]
        public void ctor_Current_IsSet(SimulationProgressEventArgs sut)
        {
            Assert.Equal<int>(1, sut.Current);
        }

        [Theory, PropertyData("Sut")]
        public void ctor_Max_IsSet(SimulationProgressEventArgs sut)
        {
            Assert.Equal<int>(2, sut.Max);
        }

        public static IEnumerable<object[]> Sut
        {
            get
            {
                yield return new object[] { new SimulationProgressEventArgs(1, 2) };
            }
        }
    }
}
