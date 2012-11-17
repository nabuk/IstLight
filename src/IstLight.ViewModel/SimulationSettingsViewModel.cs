using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using IstLight.Extensions;
using IstLight.Settings;

namespace IstLight
{
    public class SimulationSettingsViewModel : ViewModelBase
    {
        private readonly ISimulationSettings simulationSettings;
        private readonly IDictionary<CommissionType, string> commisionTypeNames;
        private readonly IDictionary<PeriodType, string> periodTypeNames;
        private readonly IDictionary<SimulationPriceType, string> simulationPriceTypeNames;
        private readonly IDictionary<SimulationRangeType, string> simulationRangeTypeNames;

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
            get { return simulationSettings.Get<AnnualInflationRateSetting>().Value; }
            set
            {
                simulationSettings.Get<AnnualInflationRateSetting>().Value = value;
                base.RaisePropertyChanged<double>(() => AnnualInflationRate);
            }
        }

        public double AnnualInterestRate
        {
            get { return simulationSettings.Get<AnnualInterestRateSetting>().Value; }
            set
            { 
                simulationSettings.Get<AnnualInterestRateSetting>().Value = value;
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
                simulationSettings.Get<CommissionSetting>().Type = commisionTypeNames.ValueToKey(value);
                base.RaisePropertyChanged<string>(() => SelectedCommissionType);
            }
        }
        public double CommissionValue
        {
            get
            {
                return simulationSettings.Get<CommissionSetting>().Value;
            }
            set
            {
                simulationSettings.Get<CommissionSetting>().Value = value;
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
