
namespace IstLight.Settings
{
    public class OnlyRecentQuotesSetting : ISimulationSetting
    {
        public OnlyRecentQuotesSetting ()
	    {
            this.Value = true;
	    }

        public bool Value { get; set; }

        public object Clone()
        {
            return new OnlyRecentQuotesSetting { Value = this.Value };
        }
    }
}
