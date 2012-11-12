using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Settings
{
    public static class SimulationPriceTypeNames
    {
        public static IDictionary<SimulationPriceType, string> TypeNames
        {
            get
            {
                return new Dictionary<SimulationPriceType, string>
                {
                    { SimulationPriceType.Open, "Open" },
                    { SimulationPriceType.Close, "Close" },
                    { SimulationPriceType.High, "High" },
                    { SimulationPriceType.Low, "Low" },
                    { SimulationPriceType.OpenCloseMiddle, "Open-Close avg." },
                    { SimulationPriceType.HighLowMiddle, "High-Low avg." }
                };
            }
        }
    }
}
