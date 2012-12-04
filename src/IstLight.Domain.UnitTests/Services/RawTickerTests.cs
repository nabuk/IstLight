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
using System.Linq;
using IstLight.Services;
using Ploeh.AutoFixture;
using Xunit;

namespace IstLight.UnitTests.Services
{
    public class RawTickerTests
    {
        [Fact]
        public void ctor_NullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RawTicker(null, "csv", new byte[0]));
        }

        [Fact]
        public void ctor_NullFormat_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RawTicker("name", null, new byte[0]));
        }

        [Fact]
        public void ctor_NullData_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RawTicker("name", "csv", null));
        }

        [Fact]
        public void Name_FromCtorArg_IsStored()
        {
            var name = Guid.NewGuid().ToString();
            var sut = new RawTicker(name, "csv", new byte[0]);
            Assert.Equal<string>(name, sut.Name);
        }

        [Fact]
        public void Format_FromCtorArg_IsStored()
        {
            var format = Guid.NewGuid().ToString();
            var sut = new RawTicker("name", format, new byte[0]);
            Assert.Equal<string>(format, sut.Format);
        }

        [Fact]
        public void Data_FromCtorArg_IsStored()
        {
            var data = new Fixture().Customize(new MultipleCustomization()).CreateMany<byte>().ToArray();
            var sut = new RawTicker("name", "format", data);
            Assert.True(data.SequenceEqual(sut.Data));
        }
        
    }
}
