using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Settings
{
    public static class CommissionTypeNames
    {
        public static IDictionary<CommissionType,string> TypeNames
        {
            get
            {
                return new Dictionary<CommissionType, string>
                {
                    { CommissionType.Percent, "Percent" },
                    { CommissionType.FixedPerTrade, "Fixed per trade" },
                    { CommissionType.FixedPerUnit, "Fixed per unit" }
                };
            }
        }
    }
}
