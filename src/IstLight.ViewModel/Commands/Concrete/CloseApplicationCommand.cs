
namespace IstLight.Commands.Concrete
{
    public class CloseApplicationCommand : GlobalCommandBase
    {
        public CloseApplicationCommand(IWindow mainWindow)
            : base("Close", new DelegateCommand(mainWindow.Close)) { }
    }
}
