
namespace IstLight.Settings
{
    public class TradeDelaySetting : ISimulationSetting
    {
        public TradeDelaySetting()
        {
            Value = 0;
        }

        public int Value { get; set; }

        public object Clone()
        {
            return new TradeDelaySetting { Value = this.Value };
        }
    }
}
