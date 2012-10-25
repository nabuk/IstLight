using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripting;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture;
using System.Threading;

namespace ScriptingWrapper.UnitTests
{
    public class ScriptEngineTests
    {
        [Theory, PropertyData("Engines")]
        public void ContainsVariable_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<ArgumentNullException>(() => engine.ContainsVariable(null));
            }
        }

        [Theory, PropertyData("Engines")]
        public void ContainsVariable_VariableDoesNotExist_ReturnsFalse(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.False(engine.ContainsVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void ContainsVariable_VariableExists_ReturnsTrue(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                Assert.True(engine.ContainsVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void SetVariable_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<ArgumentNullException>(() => engine.SetVariable(null, 0));
            }
        }

        [Theory, PropertyData("Engines")]
        public void SetVariable_SetsVariable(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                Assert.Equal<int>(1, engine.GetVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetVariableGeneric_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<ArgumentNullException>(() => engine.GetVariable<int>(null));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetVariableDynamic_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<ArgumentNullException>(() => engine.GetVariable(null));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetVariableGeneric_VariableDoesNotExist_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<MissingMemberException>(() => engine.GetVariable<int>("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetVariableDynamic_VariableDoesNotExist_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<MissingMemberException>(() => engine.GetVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetVariableGeneric_VariableWasSet_ReturnsVariable(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                Assert.Equal<int>(1, engine.GetVariable<int>("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetVariableDynamic_VariableWasSet_ReturnsVariable(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                Assert.Equal<int>(1, engine.GetVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableGeneric_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                int tVariable;
                Assert.Throws<ArgumentNullException>(() => engine.TryGetVariable<int>(null, out tVariable));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableDynamic_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                dynamic tVariable;
                Assert.Throws<ArgumentNullException>(() => engine.TryGetVariable(null, out tVariable));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableGeneric_VariableExists_ReturnsTrue(ScriptEngineBase engine)
        {
            using (engine)
            {
                int x;
                engine.SetVariable("x", 1);
                Assert.True(engine.TryGetVariable<int>("x", out x));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableGeneric_VariableDoesNotExist_ReturnsFalse(ScriptEngineBase engine)
        {
            using (engine)
            {
                int x;
                Assert.False(engine.TryGetVariable<int>("x", out x));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableDynamic_VariableExists_ReturnsTrue(ScriptEngineBase engine)
        {
            using (engine)
            {
                dynamic x;
                engine.SetVariable("x", 1);
                Assert.True(engine.TryGetVariable("x", out x));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableDynamic_VariableDoesNotExist_ReturnsFalse(ScriptEngineBase engine)
        {
            using (engine)
            {
                dynamic x;
                Assert.False(engine.TryGetVariable("x", out x));
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableGeneric_VariableExists_ReturnsCorrectValue(ScriptEngineBase engine)
        {
            using (engine)
            {
                int x;
                engine.SetVariable("x", 1);
                engine.TryGetVariable<int>("x", out x);
                Assert.Equal<int>(1, x);
            }
        }

        [Theory, PropertyData("Engines")]
        public void TryGetVariableDynamic_VariableExists_ReturnsCorrectValue(ScriptEngineBase engine)
        {
            using (engine)
            {
                dynamic x;
                engine.SetVariable("x", 1);
                engine.TryGetVariable("x", out x);
                Assert.Equal<int>(1, x);
            }
        }

        [Theory, PropertyData("Engines")]
        public void RemoveVariable_NullName_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<ArgumentNullException>(() => engine.RemoveVariable(null));
            }
        }

        [Theory, PropertyData("Engines")]
        public void RemoveVariable_RemovesVariable(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                engine.RemoveVariable("x");
                Assert.False(engine.ContainsVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void IsScriptSet_NotSet_ReturnsFalse(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.False(engine.IsScriptSet);
            }
        }

        [Theory, PropertyData("SimpleScripts")]
        public void IsScriptSet_ValidScriptWasSet_ReturnsTrue(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetScript(script);
                Assert.True(engine.IsScriptSet);
            }
        }

        [Theory, PropertyData("Engines")]
        public void SetScript_NullArg_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<ArgumentNullException>(() => engine.SetScript(null));
            }
        }

        [Theory, PropertyData("WrongCompilationScripts")]
        public void SetScript_CompilationNotPossible_ReturnsFalse(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                Assert.False(engine.SetScript(script));
            }
        }

        [Theory, PropertyData("SimpleScripts")]
        public void SetScript_ValidScript_ReturnsTrue(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                Assert.True(engine.SetScript(script));
            }
        }

        [Theory, PropertyData("SimpleScripts")]
        public void Execute_ValidScriptWasSet_ReturnsTrue(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetScript(script);
                Assert.True(engine.Execute());
            }
        }

        [Theory, PropertyData("WrongExecutionScripts")]
        public void Execute_WrongExecutionScript_ReturnsFalse(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetScript(script);
                Assert.False(engine.Execute());
            }
        }

        [Theory, PropertyData("Engines")]
        public void Execute_ScriptWasNotSet_Throws(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Throws<InvalidOperationException>(() => engine.Execute());
            }
        }

        [Theory, PropertyData("WrongExecutionScripts")]
        public void LastError_AfterWrongScriptExecution_IsNotNull(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetScript(script);
                engine.Execute();
                Assert.NotNull(engine.LastError);
            }
        }

        [Theory, PropertyData("Engines")]
        public void LastError_AtStart_IsNull(ScriptEngineBase engine)
        {
            using (engine)
            {
                Assert.Null(engine.LastError);
            }
        }

        [Theory, PropertyData("WrongCompilationScripts")]
        public void LastError_AfterSetInvalidScript_IsNotNull(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetScript(script);
                Assert.NotNull(engine.LastError);
            }
        }

        [Theory, PropertyData("Engines")]
        public void ClearScope_RemovesVariables(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                engine.ClearScope();
                Assert.False(engine.ContainsVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void GetItems_ReturnsVariables(ScriptEngineBase engine)
        {
            using (engine)
            {
                engine.SetVariable("x", 1);
                Assert.True(engine.GetItems().Select(item => new KeyValuePair<string, int>(item.Key, item.Value))
                    .SequenceEqual(new KeyValuePair<string, int>[] { new KeyValuePair<string, int>("x", 1) }), "GetItems returned unexpected sequence.");
            }
        }

        [Theory, PropertyData("Engines")]
        public void AddSearchPaths_AddsPaths(ScriptEngineBase engine)
        {
            using (engine)
            {
                var paths = new Fixture().CreateMany<string>().ToArray();
                engine.AddSearchPaths(paths.ToArray());
                Assert.True(engine.GetSearchPaths().Intersect(paths).Count() == paths.Count(), "Not all paths were add.");
            }
        }

        [Theory, PropertyData("XVariableScripts")]
        public void ScriptDoublesVariableValue_ReturnsCorrectValue(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetVariable("x", 1.0);
                engine.SetScript(script);
                engine.Execute();

                Assert.Equal<double>(2, engine.GetVariable("x"));
            }
        }

        [Theory, PropertyData("Engines")]
        public void Dispose_DoesNotThrow(ScriptEngineBase engine)
        {
            engine.Dispose();
        }

        [Theory, PropertyData("PrintXScripts")]
        public void Output_ContainsScriptOutput(ScriptEngineBase engine, string script)
        {
            using (engine)
            {
                engine.SetScript(script);
                engine.Execute();
                Assert.Equal<string>("X", engine.Output.Replace(Environment.NewLine,""));
            }
        }


        public static IEnumerable<object[]> Engines
        {
            get
            {
                foreach (ScriptingLanguage language in Enum.GetValues(typeof(ScriptingLanguage)))
                    yield return new object[] { ScriptEngineFactory.CreateEngine(language) };
            }
        }

        public static IEnumerable<object[]> SimpleScripts
        {
            get
            {
                return CombineEnginesWithScripts(new KeyValuePair<ScriptingLanguage,string>[]
                {
                    new KeyValuePair<ScriptingLanguage,string>(ScriptingLanguage.IronPython, "x = 5*5")
                });
            }
        }

        public static IEnumerable<object[]> XVariableScripts
        {
            get
            {
                return CombineEnginesWithScripts(new KeyValuePair<ScriptingLanguage, string>[]
                {
                    new KeyValuePair<ScriptingLanguage,string>(ScriptingLanguage.IronPython, "x = x*2")
                });
            }
        }

        public static IEnumerable<object[]> WrongCompilationScripts
        {
            get
            {
                return CombineEnginesWithScripts(new KeyValuePair<ScriptingLanguage, string>[]
                {
                    new KeyValuePair<ScriptingLanguage,string>(ScriptingLanguage.IronPython, "-")
                });
            }
        }

        public static IEnumerable<object[]> WrongExecutionScripts
        {
            get
            {
                return CombineEnginesWithScripts(new KeyValuePair<ScriptingLanguage, string>[]
                {
                    new KeyValuePair<ScriptingLanguage,string>(ScriptingLanguage.IronPython, "x = y")
                });
            }
        }

        public static IEnumerable<object[]> PrintXScripts
        {
            get
            {
                return CombineEnginesWithScripts(new KeyValuePair<ScriptingLanguage, string>[]
                {
                    new KeyValuePair<ScriptingLanguage,string>(ScriptingLanguage.IronPython, "print 'X'")
                });
            }
        }

        public static IEnumerable<object[]> CombineEnginesWithScripts(IEnumerable<KeyValuePair<ScriptingLanguage,string>> scripts)
        {
            if(scripts.Select(x => x.Key).Distinct().Count() != Enum.GetValues(typeof(ScriptingLanguage)).Length)
                throw new InvalidOperationException("Not all scripts were provided");

            return scripts.Select(x =>
                new object[]
                {
                    ScriptEngineFactory.CreateEngine(x.Key),
                    x.Value
                });
        }
    }
}
