using IstLight;

namespace IstLight.Scripting
{
    public interface IScriptRepository
    {
        IReadOnlyList<Script> Load();
    }
}
