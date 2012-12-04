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
