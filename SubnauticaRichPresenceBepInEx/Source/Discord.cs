using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubnauticaRichPresenceBepInEx.Source;

public static class Discord
{
    #region Public Fields

    public static Activity Activity
    {
        get {  return _activity; }
        set { _activity = value; }
    }

    #endregion

    #region Private Fields

    private static readonly Dictionary<long, DiscordObj> _clients = new();
    private static DiscordObj _currentClient;

    private static Activity _activity;

    #endregion

    private static void HandleResult(Result result, string message = "")
    {
        if (result == Result.Ok)
        {
            Logger.Log($" [{message}] Ok", true);

            return;
        }

        var stackTrace = StackTraceUtility.ExtractStackTrace();
        Logger.LogError($" [{message}Error] {result}.", true);
        Logger.LogError($"{stackTrace}", true);
    }

    public static void RegisterClient(long clientId)
    {
        if (!_clients.ContainsKey(clientId))
        {
            var client = new DiscordObj(clientId, (ulong)CreateFlags.NoRequireDiscord);
            _clients.Add(clientId, client);

            return;
        }
        Logger.LogError($"Cannot add client {clientId} because its already registered!");
    }

    public static void RegisterClientAndSwitch(long clientId)
    {
        if (!_clients.ContainsKey(clientId))
        {
            var client = new DiscordObj(clientId, (ulong)CreateFlags.NoRequireDiscord);
            _clients.Add(clientId, client);

            _currentClient = client;

            return;
        }

        Logger.LogError($"Cannot add client {clientId} because its already registered!");
    }

    public static void SwitchToClient(long clientId)
    {
        if (_clients.TryGetValue(clientId, out var client))
        {
            _currentClient = client;

            return;
        }
        Logger.LogWarn($"Client with id {clientId} isn't registered! Registering.");
        RegisterClientAndSwitch(clientId);
    }

    internal static void Update()
    {
        _currentClient.RunCallbacks();
        _currentClient.GetActivityManager().UpdateActivity(_activity, result => { HandleResult(result, "ActivityManager.UpdateActivity"); });
    }

    internal static void Dispose()
    {
        _currentClient.GetActivityManager().ClearActivity(result => { HandleResult(result, "ActivityManager.ClearActivity"); });

        // Dispose each client.
        _clients.ForEach(longClientPair => { longClientPair.Value.Dispose(); });
        _clients.Clear();

        GC.Collect();
    }
}
