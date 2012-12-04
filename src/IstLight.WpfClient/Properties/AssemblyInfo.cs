using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

[assembly: AssemblyTitle("IstLight.WpfClient")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]

public static class ExecutingAssembly
{
    public static Version Version
    {
        get
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }

    public static string Name
    {
        get
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }
    }
}
