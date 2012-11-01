using IstLight.Domain;

namespace IstLight.Scripting
{
    public interface IScriptRepository
    {
        IReadOnlyList<Script> Load();
    }
}
