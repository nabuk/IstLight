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
