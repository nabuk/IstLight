using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Settings
{
    public static class PeriodTypeNames
    {
        public static IDictionary<PeriodType, string> TypeNames
        {
            get
            {
                return Enum.GetValues(typeof(PeriodType)).Cast<PeriodType>().ToDictionary(x => x, x => x.ToString());
            }
        }
    }
}
