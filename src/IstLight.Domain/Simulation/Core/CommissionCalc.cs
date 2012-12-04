// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using IstLight.Settings;

namespace IstLight.Simulation.Core
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
