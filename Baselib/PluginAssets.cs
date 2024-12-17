using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UWE;

namespace Baselib;

/// <summary>
/// Assets from this plugin.
/// </summary>
internal class PluginAssets
{
    private static AssetBundle _bundle;
    private static bool _isInitialized = false;

    /// <summary>
    /// Initializes the assets (in case they haven't) and returns the asset bundle.
    /// </summary>
    /// <returns>The asset bundle or null if not initialized.</returns>
    public static AssetBundle Get()
    {
        Initialize();
        return _bundle;
    }

    /// <summary>
    /// Initializes the assets asynchronously
    /// </summary>
    public static void Initialize()
    {
        if (_isInitialized) return;
        CoroutineHost.StartCoroutine(InitializeAsync());
        _isInitialized = true;
    }

    private static IEnumerator InitializeAsync()
    {
        var modAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var bundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(modAssemblyLocation, "customgeneratorpieces"));
        yield return bundleRequest;
        _bundle = bundleRequest.assetBundle;
    }
}
