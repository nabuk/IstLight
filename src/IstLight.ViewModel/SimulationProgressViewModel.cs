using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Simulation;

namespace IstLight
{
    public class SimulationProgressViewModel : ViewModelBase
    {
        private SimulationProgressStatus status;

        public SimulationProgressStatus Status
        {
            get { return status; }
            internal set
            {
                status = value;
                base.RaisePropertyChanged<SimulationProgressStatus>(() => Status);
            }
        }
    }
}
