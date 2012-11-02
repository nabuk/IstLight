using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

namespace IstLight.UnitTests
{
    public class AutoMockingDataAttribute : AutoDataAttribute
    {
        public AutoMockingDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization())) { }
    }
}
