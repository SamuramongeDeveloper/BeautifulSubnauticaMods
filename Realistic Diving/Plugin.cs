using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace RealisticDiving;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    private static Harmony _harmony = new(PluginInfo.PLUGIN_NAME);

    private void Awake()
    {
        Logger = base.Logger;
        
        _harmony.PatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo("Realistic Diving has loaded correctly!");
    }
}