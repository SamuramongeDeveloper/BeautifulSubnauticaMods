using System;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace SubnauticaRichPresenceBepInEx;

internal static class Paths
{
    #region Public Fields

    public static string AssemblyDirectory
    {
        get
        {
            _assemblyDir ??= GetAssemblyDirectory();
            return _assemblyDir;
        }
    }

    public static string X86
    {
        get
        {
            _x86 ??= Path.Combine(AssemblyDirectory, "x86");
            return _x86;
        }
    }

    public static string X86_64
    {
        get
        {
            _x86_64 ??= Path.Combine(AssemblyDirectory, "x86_64");
            return _x86_64;
        }
    }


    public static string Aarch64
    {
        get
        {
            _aarch64 ??= Path.Combine(AssemblyDirectory, "aarch64");
            return _aarch64;
        }
    }

    #endregion

    #region Private Fields

    private static string _assemblyDir;

    private static string _x86;

    private static string _x86_64;

    private static string _aarch64;

    #endregion

    private static string GetAssemblyDirectory()
    {
        var codeBase = Assembly.GetExecutingAssembly().CodeBase;
        var uri = new UriBuilder(codeBase);
        var path = Uri.UnescapeDataString(uri.Path);

        return Path.GetDirectoryName(path);
    }

    [DllImport("kernel32.dll")]
    public static extern bool SetDllDirectoryA(string lpPathName = "");
}
