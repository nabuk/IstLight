using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using IstLight.Extensions;

namespace IstLight.UnitTests.Extensions
{
    public class GeneralExtensionsTests
    {
        [Fact]
        public void TypedClone_Clone_IsCalledOnce()
        {
            var mock = new Mock<ICloneable>();
            mock.Setup(x => x.Clone()).Returns(mock.Object);

            GeneralExtensions.TypedClone(mock.Object);

            mock.Verify(x => x.Clone(), Times.Once());
        }

        [Fact]
        public void TypedClone_Clone_ReturnsCloneObject()
        {
            var mock = new Mock<ICloneable>();
            mock.Setup(x => x.Clone()).Returns(mock.Object);

            var result = GeneralExtensions.TypedClone(mock.Object);

            Assert.Same(mock.Object, result);
        }
    }
}
