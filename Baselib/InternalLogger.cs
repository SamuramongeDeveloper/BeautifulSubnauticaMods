using BepInEx.Logging;
using System;

namespace Baselib;

internal static class InternalLogger
{
    private static  ManualLogSource _source;

    public static void Initialize(ManualLogSource logSource) => _source = logSource;

    public static void Info(object message)
    {
        string msg = Stringify(message);
        Log(LogLevel.Info, msg);
    }

    public static void Error(object message)
    {
        string msg = Stringify(message);
        Log(LogLevel.Error, msg);
    }

    public static void Throw(object message)
    {
        string msg = Stringify(message);
        throw new UnexpectedException(msg);
    }

    private static string Stringify(object value)
    {
        if (value is string) 
            return value as string;
        else return value.ToString();
    }

    private static void Log(LogLevel level, string message) => _source.Log(level, message);
}

/// <summary>
/// When things get strange and somehow you need to throw an exception even if its never possible.
/// </summary>
public class UnexpectedException : Exception
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="message"><inheritdoc/></param>
    public UnexpectedException(string message) : base(message) { }
}