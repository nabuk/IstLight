using System;
using IstLight.Simulation;
using Xunit;

namespace IstLight.UnitTests.Simulation
{
    public class SimulationEndEventArgsTests
    {
        [Fact]
        public void HasDefaultCtor()
        {
            Assert.True(typeof(SimulationEndEventArgs).HasDefaultConstructor());
        }

        [Fact]
        public void EndReason_CanBeSet()
        {
            var sut = CreateSut();
            var endReason = SimulationEndReason.Completion;
            sut.EndReason = endReason;

            Assert.Equal<SimulationEndReason>(endReason, sut.EndReason);
        }

        [Fact]
        public void Result_CanBeSet()
        {
            var sut = CreateSut();
            var result = SimulationResultTests.CreateSut().Item1;
            sut.Result = result;

            Assert.Equal<SimulationResult>(result, sut.Result);
        }

        [Fact]
        public void Error_CanBeSet()
        {
            var sut = CreateSut();
            var error = Guid.NewGuid().ToString();
            sut.Error = error;

            Assert.Equal<string>(error, sut.Error);
        }

        public static SimulationEndEventArgs CreateSut()
        {
            return new SimulationEndEventArgs();
        }
    }
}
