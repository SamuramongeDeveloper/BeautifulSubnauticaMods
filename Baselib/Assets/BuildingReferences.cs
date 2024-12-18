using System.Collections;
using UnityEngine;
using UWE;
using UnityObject = UnityEngine.Object;

namespace Baselib.Assets;

/// <summary>
/// Stores references related to the <seealso cref="ConstructableBase"/> component.
/// </summary>
public class BuildingReferences
{
    private static GameObject _builtBoxFX;
    private static Texture _emissiveTex;
    private static Texture _noiseTex;

    /// <summary>
    /// The x_BuiltBoxFX GameObject.
    /// </summary>
    public static GameObject BuiltBoxFX
    {
        get
        {
            var copy = UWE.Utils.InstantiateDeactivated(_builtBoxFX);
            return copy;
        }
    }

    /// <summary>
    /// The emissive texuture.
    /// </summary>
    public static Texture EmissiveTexture => _emissiveTex;

    /// <summary>
    /// The noise texture.
    /// </summary>
    public static Texture NoiseTexture => _noiseTex;

    /// <summary>
    /// Initializes the references.
    /// </summary>
    internal static void InitializeReferences()
    {
        CoroutineHost.StartCoroutine(InitializeReferencesAsync());
    }

    private static IEnumerator InitializeReferencesAsync()
    {
        var request = CraftData.GetPrefabForTechTypeAsync(TechType.BaseCorridorI);
        yield return request;
        var result = request.result;
        
        var prefab = result.Get();
        var constructableBase = prefab.GetComponent<ConstructableBase>();

        _builtBoxFX = UWE.Utils.InstantiateDeactivated(constructableBase.builtBoxFX);
        _builtBoxFX.transform.SetParent(Plugin.GameObjectPreserver.transform, false); // Prevent this bad boy getting annihilated by the scene cleaner.

        _emissiveTex = constructableBase._EmissiveTex;
        _noiseTex = constructableBase._NoiseTex;
    }
}
