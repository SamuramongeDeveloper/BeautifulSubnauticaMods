using UnityEngine;

namespace Baselib.Assets;

/// <summary>
/// A template with settings related to generating the base piece's ghost. Cannot be inherited.
/// </summary>
public sealed partial  class GhostTemplate
{
    /// <summary>
    /// The prefab of the base piece's ghost.
    /// </summary>
    public GameObject Prefab { get; }

    /// <summary>
    /// Defines the data for a <seealso cref="ConstructableBase"/> component.
    /// </summary>
    public ConstructableData MonoConstructable { get; set; }

    /// <summary>
    /// Defines the data for a <seealso cref="BaseGhost"/> component.
    /// </summary>
    public BaseGhostData MonoGhostData { get; set; }

    /// <summary>
    /// Constructs a ghost template object.
    /// </summary>
    /// <param name="prefab">The desired prefab to construct to.</param>
    public GhostTemplate(GameObject prefab) => Prefab = prefab;
}

public sealed partial class GhostTemplate
{
    private static GameObject _templatesHolder;
    private static GameObject _ghostGenTemplate;
    private static bool _initialized = false;

    /// <summary>
    /// Returns a ghost generator template, to use in the constructor of this class.
    /// <para>
    /// <br/>Consists of a root GameObject with <seealso cref="PrefabIdentifier"/> and <seealso cref="ConstructableBase"/> components, and a
    /// <br/>GameObject called "BaseGhost", with the <seealso cref="Base"/> and <seealso cref="BaseGhost"/> components.
    /// </para>
    /// </summary>
    public static GameObject GhostGeneratorTemplate => _ghostGenTemplate;

    /// <summary>
    /// Initializes the ghost generator prefabs used as templates.
    /// </summary>
    internal static void InitializeTemplatePrefabs()
    {
        if (_initialized) return;

        InternalLogger.Info("GhostGeneratorTemplate setup starting...");
        _ghostGenTemplate = new GameObject("GhostGeneratorTemplate");
        _ghostGenTemplate.transform.SetParent(Plugin.GameObjectPreserver.transform, false); // Prevent the gameobject from bein cleansed from its sins, therefore not existing anymore.
        
        _ghostGenTemplate.AddComponent<PrefabIdentifier>();

        var baseGhost = new GameObject("BaseGhost");
        baseGhost.transform.SetParent(_ghostGenTemplate.transform, false); // Parent is inactive, this is too.

        var @base = baseGhost.AddComponent<Base>();
        @base.isGhost = true;
        
        InternalLogger.Info("GhostGeneratorTemplate setup was successful.");
        _initialized = true;
    }
}