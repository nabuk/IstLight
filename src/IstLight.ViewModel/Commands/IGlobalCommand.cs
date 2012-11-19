using System.Windows.Input;

namespace IstLight.Commands
{
    public interface IGlobalCommand : ICommand
    {
        string Key { get; }
    }
}
