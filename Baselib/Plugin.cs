using Baselib.Assets;
using Baselib.TestMod;
using Nautilus.Assets;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace Baselib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    private static readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);
    private static readonly string AssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public static GameObject GameObjectPreserver { get; private set; }

    public static AssetBundle PrecursorBasesBundle { get; } = AssetBundle.LoadFromFile(Path.Combine(AssemblyLocation, "precursorassets"));

    public static AssetBundle GeneratorPiecesBundle { get; } = AssetBundle.LoadFromFile(Path.Combine(AssemblyLocation, "customgeneratorpieces"));

    private void Awake()
    {
        InternalLogger.Initialize(Logger);

        InitializePreserver();
        BuildingReferences.InitializeReferences();
        GhostTemplate.InitializeTemplatePrefabs();
        new PrecursorICorridor(PrefabInfo.WithTechType("precursoricorridor", "Alien I Comparment", "An alien I compartment, synthetized with magic, hopes and dreams.")).Register();

        _harmony.PatchAll();

        InternalLogger.Info("Initialized correctly! or not.");
    }

    private void InitializePreserver()
    {
        GameObjectPreserver = new GameObject("BaseLibGameObjectPreserver");
        GameObjectPreserver.transform.SetParent(transform, false);
        GameObjectPreserver.SetActive(false);
    }
}