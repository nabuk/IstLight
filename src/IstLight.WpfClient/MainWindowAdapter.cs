using System.Windows;

namespace IstLight
{
    public class MainWindowAdapter : WindowAdapter
    {
        public MainWindowAdapter(Window mainWindow) : base(mainWindow) { }

        public void AcceptViewModel(object viewModel)
        {
            base.wpfWindow.DataContext = viewModel;
        }
    }
}
