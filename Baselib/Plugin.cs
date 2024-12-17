using Baselib.TestMod;
using Baselib.TestPieces;
using BepInEx;
using HarmonyLib;

namespace Baselib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    private static readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        InternalLogger.Initialize(Logger);

        PluginAssets.Initialize();
        PrecursorAssets.Initialize();

        PrecursorICorridor.Register();

        InternalLogger.Info("Initialized correctly! or not.");
    }
}