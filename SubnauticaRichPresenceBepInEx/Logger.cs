using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using BepInLogLevel = BepInEx.Logging.LogLevel;

namespace SubnauticaRichPresenceBepInEx;

internal static class Logger
{
    #region Private Fields

    private static ManualLogSource _logger;

    private static List<string> _logFile;

    #endregion

    private static void Log(BepInLogLevel level, object message, bool toFile)
    {
        if (_logger != null)
        {
            var str = message.ToString();

            if (toFile)
            {
                _logFile.Add($"[{DateTime.Now}] [{level}] [Discord]: {str}");
            }
            else
            {
                _logger.Log(level, str);
            }
        }
    }

    public static void Init(ManualLogSource manualLogSource)
    {
        _logger = manualLogSource;

        _logFile = new();

        Log("Initialized Logs!", true);
    }

    public static void Log(object message, bool toFile = false)
    {
        Log(BepInLogLevel.Info, message, toFile);      
    }

    public static void LogWarn(object message, bool toFile = false)
    {
        Log(BepInLogLevel.Warning, message, toFile);

    }

    public static void LogError(object message, bool toFile = false)
    {
        Log(BepInLogLevel.Error, message, toFile);

    }

    public static void SaveLogFile()
    {
        var assemblyDirectory = Paths.AssemblyDirectory;
        var saveFile = Path.Combine(assemblyDirectory, "DiscordOutput.log");

        Log(saveFile);

        File.WriteAllLines(saveFile, _logFile);
    }
}
