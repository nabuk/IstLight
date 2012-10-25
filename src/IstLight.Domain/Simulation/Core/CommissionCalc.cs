using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Settings;

namespace IstLight.Domain.Simulation.Core
{
    public static class CommissionCalc
    {
        public static double ComputeFee(this CommissionSetting commissionSetting, double quantity, double unitPrice)
        {
            quantity = Math.Abs(quantity);
            switch (commissionSetting.Type)
            {
                case CommissionType.FixedPerTrade: return commissionSetting.Value;
                case CommissionType.FixedPerUnit: return quantity * commissionSetting.Value;
                case CommissionType.Percent: return (quantity * unitPrice) * commissionSetting.Value;
                default: throw new NotImplementedException();
            }
        }

        public static double MaxQuantity(this CommissionSetting commissionSetting, double unitPrice, double cash)
        {
            if (cash <= 0) return 0;

            switch (commissionSetting.Type)
            {
                case CommissionType.FixedPerTrade: return Math.Max((cash - commissionSetting.Value) / unitPrice, 0);
                case CommissionType.FixedPerUnit: return cash / (commissionSetting.Value + unitPrice);
                case CommissionType.Percent: return cash / (unitPrice * (1.0 + commissionSetting.Value));
                default: throw new NotImplementedException();
            }

        }
    }
}
