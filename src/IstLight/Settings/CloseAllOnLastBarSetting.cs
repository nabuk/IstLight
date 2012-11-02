
namespace IstLight.Settings
{
    public class CloseAllOnLastBarSetting : ISimulationSetting
    {
        public CloseAllOnLastBarSetting()
        {
            Value = true;
        }

        public bool Value { get; set; }

        public object Clone()
        {
            return new CloseAllOnLastBarSetting { Value = this.Value };
        }
    }
}
