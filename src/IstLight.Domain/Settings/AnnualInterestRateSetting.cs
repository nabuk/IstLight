
namespace IstLight.Settings
{
    public class AnnualInterestRateSetting : ISimulationSetting
    {
        public AnnualInterestRateSetting()
        {
            Value = 0.02;
        }

        public double Value { get; set; }

        public object Clone()
        {
            return new AnnualInterestRateSetting { Value = this.Value };
        }
    }
}
