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
using Ploeh.AutoFixture;
using Xunit;

namespace IstLight.Scripting.UnitTests
{
    public class ScriptTests
    {
        [Fact]
        public void ctor_NullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateSut(name: null));
        }

        [Fact]
        public void ctor_EmptyName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateSut(name: ""));
        }

        [Fact]
        public void ctor_NullExtension_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateSut(extension: null));
        }

        [Fact]
        public void ctor_EmptyExtension_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateSut(extension: ""));
        }

        [Fact]
        public void ctor_NullContent_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CreateSut(content: null));
        }

        [Fact]
        public void Name_PassetToCtor_IsSet()
        {
            var x = new Fixture().CreateAnonymous<string>();
            var sut = CreateSut(name: x);
            Assert.Equal<string>(x, sut.Name);
        }

        [Fact]
        public void Extension_PassetToCtor_IsSet()
        {
            var x = new Fixture().CreateAnonymous<string>();
            var sut = CreateSut(extension: x);
            Assert.Equal<string>(x, sut.Extension);
        }

        [Fact]
        public void Content_PassetToCtor_IsSet()
        {
            var x = new Fixture().CreateAnonymous<string>();
            var sut = CreateSut(content: x);
            Assert.Equal<string>(x, sut.Content);
        }

        public Script CreateSut(string name = "default", string extension = "py", string content = "")
        {
            return new Script(name, extension, content);
        }
    }
}
