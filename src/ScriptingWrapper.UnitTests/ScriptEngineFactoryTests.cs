using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace ScriptingWrapper.UnitTests
{
    public class ScriptEngineFactoryTests
    {
        [Theory, PropertyData("ScriptingLanguages")]
        public void CreateEngine_ReturnsNotNull(ScriptingLanguage lang)
        {
            Assert.NotNull(ScriptEngineFactory.CreateEngine(lang));
        }

        [Theory, PropertyData("ScriptingExtensions")]
        public void TryCreateEngine_ValidExtension_ReturnsNotNull(string extension)
        {
            Assert.NotNull(ScriptEngineFactory.TryCreateEngine(extension));
        }

        [Fact]
        public void TryCreateEngine_NullArg_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ScriptEngineFactory.TryCreateEngine(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("wtf")]
        public void TryCreateEngine_InvalidExtension_ReturnsNull(string extension)
        {
            Assert.Null(ScriptEngineFactory.TryCreateEngine(extension));
        }

        [Fact]
        public void ExtensionToLanguageMap_AllLanguagesAreListed()
        {
            var sut = ScriptEngineFactory.ExtensionToLanguageMap.Select(x => x.Value).Distinct().OrderBy(x => x);
            var expected = Enum.GetValues(typeof(ScriptingLanguage)).Cast<ScriptingLanguage>().OrderBy(x => x);
            Assert.True(sut.SequenceEqual(expected));
        }

        [Fact]
        public void AllowedExtensions_ReturnsSameExtensionsAsExtensionToLanguageMap()
        {
            var sut = ScriptEngineFactory.AllowedExtensions.OrderBy(x => x);
            var expected = ScriptEngineFactory.ExtensionToLanguageMap.Select(x => x.Key).OrderBy(x => x);
            Assert.True(sut.SequenceEqual(expected));
        }


        public static IEnumerable<object[]> ScriptingLanguages
        {
            get
            {
                foreach (ScriptingLanguage lang in Enum.GetValues(typeof(ScriptingLanguage)))
                    yield return new object[] { lang };
            }
        }
        public static IEnumerable<object[]> ScriptingExtensions
        {
            get
            {
                return ScriptEngineFactory.AllowedExtensions.Select(ext => new object[] { ext });
            }
        }
    }
}
