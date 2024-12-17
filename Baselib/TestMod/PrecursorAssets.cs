using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UWE;

namespace Baselib.TestPieces;

public static class PrecursorAssets
{
    private static AssetBundle _bundle;
    private static bool _isInitialized = false;

    public static bool IsInitialized() => _isInitialized;

    public static AssetBundle Get()
    {
        Initialize();
        return _bundle;
    }

    public static void Initialize()
    {
        if (_isInitialized) return;
        CoroutineHost.StartCoroutine(InitializeAsync());
        _isInitialized = true;
    }

    private static IEnumerator InitializeAsync()
    {
        var modAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var bundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(modAssemblyLocation, "precursorassets"));
        yield return bundleRequest;
        _bundle = bundleRequest.assetBundle;
    }
}