using System.Collections.Generic;

namespace IstLight.Settings
{
    public class SimulationRangeTypeNames
    {
        public static IDictionary<SimulationRangeType, string> TypeNames
        {
            get
            {
                return new Dictionary<SimulationRangeType, string>
                {
                    { SimulationRangeType.Common, "Common" },
                    { SimulationRangeType.FromToDates, "From-To" }
                };
            }
        }
    }
}
