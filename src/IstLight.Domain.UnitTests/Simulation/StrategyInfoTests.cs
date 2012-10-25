using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Simulation;
using Xunit;
using Xunit.Extensions;

namespace IstLight.Domain.UnitTests.Simulation
{
    public class StrategyInfoTests
    {
        [Fact]
        public void ctor_Script_IsSet()
        {
            var script = ScriptTests.CreateScript();
            var sut = CreateStrategyInfo(script: script);

            Assert.Equal<Script>(script, sut.Script);
        }

        [Fact]
        public void ctor_Ouput_IsSet()
        {
            var output = Guid.NewGuid().ToString();
            var sut = CreateStrategyInfo(output: output);

            Assert.Equal<string>(output, sut.Output);
        }

        public static StrategyInfo CreateStrategyInfo(Script script = null, string output = null)
        {
            return new StrategyInfo(script ?? ScriptTests.CreateScript(), output ?? Guid.NewGuid().ToString());
        }

    }
}
