using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using SeaStraw.Items;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeaStraw;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class SeaStraw : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }
    public static new Config Config { get; } = OptionsPanelHandler.RegisterModOptions<Config>();
    private static Harmony Harmony{ get; } = new(PluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        Logger = base.Logger;

        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        SeaStrawPrefab.Register();
        RefillerChipPrefab.Register();

        Logger.LogInfo($"Plugin loaded.");
    }
}