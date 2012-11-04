using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
