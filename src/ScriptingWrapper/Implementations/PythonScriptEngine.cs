
namespace Scripting.Implementations
{
    class PythonScriptEngine : ScriptEngineBase
    {
        public PythonScriptEngine() : base(IronPython.Hosting.Python.CreateEngine()) { }

        public override ScriptingLanguage Language
        {
            get { return ScriptingLanguage.IronPython; }
        }
    }
}
