using System.Windows.Input;
using GalaSoft.MvvmLight;
using IstLight.Simulation;

namespace IstLight.ViewModels
{
    public class SimulationProgressViewModel : ViewModelBase
    {
        private SimulationProgressStatus status;

        public SimulationProgressViewModel(ICommand cancelCommand)
        {
            this.CancelCommand = cancelCommand;
        }

        public SimulationProgressStatus Status
        {
            get { return status; }
            internal set
            {
                status = value;
                base.RaisePropertyChanged<SimulationProgressStatus>(() => Status);
            }
        }

        public ICommand CancelCommand { get; private set; }
    }
}