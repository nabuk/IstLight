using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture;

namespace IstLight.Domain.UnitTests
{
    public class ScriptTests
    {
        [Fact]
        public void CreateByLanguage_NullLanguage_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Script.CreateByLanguage(null));
        }

        [Fact]
        public void CreateByExtension_NullExtension_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Script.CreateByExtension(null));
        }

        [Fact]
        public void CreateByLanguage_InvalidLanguage_ReturnsNull()
        {
            var sut = Script.CreateByLanguage("__ invalid language __");
            Assert.Null(sut);
        }

        [Fact]
        public void CreateByExtension_InvalidExtension_ReturnsNull()
        {
            var sut = Script.CreateByExtension("__ invalid extensions __");
            Assert.Null(sut);
        }

        [Theory]
        [ClassData(typeof(ScriptLangExtTestDataProvider))]
        public void CreateByLanguage_ValidLanguage_ReturnsConcreteWithSetLanguageAndExtension(string language, string extension)
        {
            var sut = Script.CreateByLanguage(language);

            Assert.True(
                string.Equals(language, sut.Language, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(extension, sut.Extension, StringComparison.InvariantCultureIgnoreCase),
                "Language and/or extension is incorrect.");
        }

        [Theory]
        [ClassData(typeof(ScriptLangExtTestDataProvider))]
        public void CreateByExtension_ValidExtension_ReturnsConcreteWithSetLanguageAndExtension(string language, string extension)
        {
            var sut = Script.CreateByExtension(extension);

            Assert.True(
                string.Equals(language, sut.Language, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(extension, sut.Extension, StringComparison.InvariantCultureIgnoreCase),
                "Language and/or extension is incorrect.");
        }

        [Fact]
        public void Name_CanBeChanged()
        {
            string newName = new Fixture().CreateAnonymous<string>();
            
            var sut = CreateScript();
            sut.Name = newName;

            Assert.Equal<string>(newName, sut.Name);
        }

        [Fact]
        public void Content_CanBeChanged()
        {
            string newContent = new Fixture().CreateAnonymous<string>();

            var sut = CreateScript();
            sut.Content = newContent;

            Assert.Equal<string>(newContent, sut.Content);
        }

        public static Script CreateScript()
        {
            return Script.CreateByLanguage(Script.ExtensionToLanguageMap.First().Value);
        }

        public class ScriptLangExtTestDataProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (var extToLang in Script.ExtensionToLanguageMap)
                    yield return new object[] { extToLang.Value, extToLang.Key };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
