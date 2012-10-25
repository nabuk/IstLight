using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

namespace IstLight.Domain.UnitTests
{
    public class AutoMockingDataAttribute : AutoDataAttribute
    {
        public AutoMockingDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization())) { }
    }
}
