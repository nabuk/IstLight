using System.Windows;

namespace IstLight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Bootstrapper.CompositionRoot.GetEntryPoint().Enter();
        }
    }
}
