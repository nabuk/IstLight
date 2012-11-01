using IstLight;

namespace IstLight.Scripting
{
    public interface IScriptService
    {
        IReadOnlyList<Script> Load();
    }
}
