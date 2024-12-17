global using DiscordRPC;
global using DiscordObj = DiscordRPC.Discord;

using BepInEx;
using HarmonyLib;
using SubnauticaRichPresenceBepInEx.Source;
using System;
using System.Reflection;
using UnityEngine;
using DiscordRp = SubnauticaRichPresenceBepInEx.Source.Discord;
using DLogger = SubnauticaRichPresenceBepInEx.Logger; 

namespace SubnauticaRichPresenceBepInEx;

[BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        DLogger.Init(Logger);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        // Ensures that the correct discord_game_sdk is going to be loaded.
        if (Environment.Is64BitOperatingSystem)
            Paths.SetDllDirectoryA(Paths.X86_64);
        else if (!Environment.Is64BitOperatingSystem)
            Paths.SetDllDirectoryA(Paths.X86);
        else if (Environment.OSVersion.Platform == PlatformID.Unix)
            Paths.SetDllDirectoryA(Paths.Aarch64);

        // Initializes discord.
        // 1073381647287853097 (Modded default discord client)
        DiscordRp.RegisterClientAndSwitch(1073381647287853097);

        var discordGameObject = new GameObject("DiscordMono");
        discordGameObject.AddComponent<DiscordMono>();
        discordGameObject.AddComponent<SceneCleanerPreserve>();

        DontDestroyOnLoad(discordGameObject);

        // After initializing discord, the dll is already loaded so we don't need the dll path anymore.
        Paths.SetDllDirectoryA();

        DLogger.Log("Plugin Subnautica Rich Presence has initialized correctly.");
    }

    private void OnApplicationQuit()
    {
        DLogger.SaveLogFile();
    }
}
