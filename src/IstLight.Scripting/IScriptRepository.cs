using IstLight;

namespace IstLight
{
    public interface IScriptService
    {
        IReadOnlyList<Script> Load();
    }
}
