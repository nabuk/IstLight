
namespace IstLight.Settings
{
    public class AnnualInflationRateSetting : ISimulationSetting
    {
        public AnnualInflationRateSetting()
        {
            Value = 0.03;
        }

        public double Value { get; set; }

        public object Clone()
        {
            return new AnnualInflationRateSetting { Value = this.Value };
        }
    }
}
