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
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using IstLight.Extensions;
using IstLight.Settings;

namespace IstLight.ViewModels
{
    public class SimulationSettingsViewModel : ViewModelBase
    {
        private readonly ISimulationSettings simulationSettings;
        private readonly IDictionary<CommissionType, string> commisionTypeNames;
        private readonly IDictionary<PeriodType, string> periodTypeNames;
        private readonly IDictionary<SimulationPriceType, string> simulationPriceTypeNames;
        private readonly IDictionary<SimulationRangeType, string> simulationRangeTypeNames;
        private double CommissionTypeMultiplier
        {
            get
            {
                return simulationSettings.Get<CommissionSetting>().Type == CommissionType.Percent ? 100 : 1;
            }
        }

        public SimulationSettingsViewModel(ISimulationSettings simulationSettings)
        {
            this.simulationSettings = simulationSettings;
            this.commisionTypeNames = CommissionTypeNames.TypeNames;
            this.periodTypeNames = PeriodTypeNames.TypeNames;
            this.simulationPriceTypeNames = SimulationPriceTypeNames.TypeNames;
            this.simulationRangeTypeNames = SimulationRangeTypeNames.TypeNames;
            this.SimulationRangeFrom = DateTime.Now.AddYears(-10);
            this.SimulationRangeTo = DateTime.Now;
        }

        public double AnnualInflationRate
        {
            get { return simulationSettings.Get<AnnualInflationRateSetting>().Value * 100; }
            set
            {
                simulationSettings.Get<AnnualInflationRateSetting>().Value = value / 100;
                base.RaisePropertyChanged<double>(() => AnnualInflationRate);
            }
        }

        public double AnnualInterestRate
        {
            get { return simulationSettings.Get<AnnualInterestRateSetting>().Value * 100; }
            set
            { 
                simulationSettings.Get<AnnualInterestRateSetting>().Value = value / 100;
                base.RaisePropertyChanged<double>(() => AnnualInterestRate);
            }
        }

        public bool CloseAllOnLastBar
        {
            get { return simulationSettings.Get<CloseAllOnLastBarSetting>().Value; }
            set
            {
                simulationSettings.Get<CloseAllOnLastBarSetting>().Value = value;
                base.RaisePropertyChanged<bool>(() => CloseAllOnLastBar);
            }
        }

        public IEnumerable<string> CommissionTypes { get { return commisionTypeNames.Values; } }
        public string SelectedCommissionType
        {
            get
            {
                return commisionTypeNames[simulationSettings.Get<CommissionSetting>().Type];
            }
            set
            {
                var valueBeforeChange = CommissionValue;
                simulationSettings.Get<CommissionSetting>().Type = commisionTypeNames.ValueToKey(value);
                CommissionValue = valueBeforeChange;
                base.RaisePropertyChanged<string>(() => SelectedCommissionType);
            }
        }
        public double CommissionValue
        {
            get
            {
                return simulationSettings.Get<CommissionSetting>().Value * CommissionTypeMultiplier;
            }
            set
            {
                simulationSettings.Get<CommissionSetting>().Value = value / CommissionTypeMultiplier;
                base.RaisePropertyChanged<double>(() => CommissionValue);
            }
        }

        public double InitialEquity
        {
            get
            {
                return simulationSettings.Get<InitialEquitySetting>().Value;
            }
            set
            {
                simulationSettings.Get<InitialEquitySetting>().Value = value;
                base.RaisePropertyChanged<double>(() => InitialEquity);
            }
        }

        public bool OnlyRecentQuotes
        {
            get
            {
                return simulationSettings.Get<OnlyRecentQuotesSetting>().Value;
            }
            set
            {
                simulationSettings.Get<OnlyRecentQuotesSetting>().Value = value;
                base.RaisePropertyChanged<bool>(() => OnlyRecentQuotes);
            }
        }

        public IEnumerable<string> PeriodTypes { get { return periodTypeNames.Values; } }
        public string SelectedPeriodType
        {
            get
            {
                return periodTypeNames[simulationSettings.Get<PeriodSetting>().Type];
            }
            set
            {
                simulationSettings.Get<PeriodSetting>().Type = periodTypeNames.ValueToKey(value);
                base.RaisePropertyChanged<string>(() => SelectedPeriodType);
            }
        }

        public IEnumerable<string> SimulationPriceTypes { get { return simulationPriceTypeNames.Values; } }
        public string SelectedSimulationPriceType
        {
            get
            {
                return simulationPriceTypeNames[simulationSettings.Get<SimulationPriceSetting>().Type];
            }
            set
            {
                simulationSettings.Get<SimulationPriceSetting>().Type = simulationPriceTypeNames.ValueToKey(value);
                base.RaisePropertyChanged<string>(() => SelectedSimulationPriceType);
            }
        }

        public IEnumerable<string> SimulationRangeTypes { get { return simulationRangeTypeNames.Values; } }
        public string SelectedSimulationRangeType
        {
            get
            {
                return simulationRangeTypeNames[simulationSettings.Get<SimulationRangeSetting>().Type];
            }
            set
            {
                simulationSettings.Get<SimulationRangeSetting>().Type = simulationRangeTypeNames.ValueToKey(value);
                base.RaisePropertyChanged<string>(() => SelectedSimulationRangeType);
                base.RaisePropertyChanged<bool>(() => CanEditSimulationRangeDates);
            }
        }
        public DateTime SimulationRangeFrom
        {
            get { return simulationSettings.Get<SimulationRangeSetting>().From; }
            set
            {
                simulationSettings.Get<SimulationRangeSetting>().From = value;
                base.RaisePropertyChanged<DateTime>(() => SimulationRangeFrom);
            }
        }
        public DateTime SimulationRangeTo
        {
            get { return simulationSettings.Get<SimulationRangeSetting>().To; }
            set
            {
                simulationSettings.Get<SimulationRangeSetting>().To = value;
                base.RaisePropertyChanged<DateTime>(() => SimulationRangeTo);
            }
        }
        public bool CanEditSimulationRangeDates
        {
            get
            {
                return simulationSettings.Get<SimulationRangeSetting>().Type == SimulationRangeType.FromToDates;
            }
        }

        public int TradeDelay
        {
            get
            {
                return simulationSettings.Get<TradeDelaySetting>().Value;
            }
            set
            {
                simulationSettings.Get<TradeDelaySetting>().Value = value;
                base.RaisePropertyChanged<int>(() => TradeDelay);
            }
        }

        internal ISimulationSettings GetSimulationSettings()
        {
            return new SimulationSettingsImmutableDecorator(simulationSettings.TypedClone());
        }
    }
}
