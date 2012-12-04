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

using IstLight.Strategy;
using Xunit;

namespace IstLight.UnitTests.Strategy
{
    public class EmptyStrategyTests
    {
        [Fact]
        public void Initialize_ReturnsTrue()
        {
            var sut = new EmptyStrategy();
            Assert.True(sut.Initialize());
        }

        [Fact]
        public void Run_ReturnsTrue()
        {
            var sut = new EmptyStrategy();
            Assert.True(sut.Run());
        }

        [Fact]
        public void LastError_IsNull()
        {
            var sut = new EmptyStrategy();
            Assert.Null(sut.LastError);
        }

        [Fact]
        public void Dispose_DoesNotThrow()
        {
            var sut = new EmptyStrategy();
            Assert.DoesNotThrow(() => sut.Dispose());
        }
    }
}
