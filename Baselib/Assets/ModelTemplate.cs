using System.Collections;
using UnityEngine;

namespace Baselib.Assets;

/// <summary>
/// A template with the settings to set up a model-only prefab. Cannot be inherited.
/// </summary>
public sealed class ModelTemplate
{
    /// <summary>
    /// This model's prefab.
    /// </summary>
    public GameObject Prefab { get; }

    /// <summary>
    /// The model's rotation.
    /// </summary>
    public Quaternion Rotation { get; set; }

    /// <summary>
    /// Constructs a model template object. 
    /// </summary>
    /// <param name="prefab">The prefab of the model.</param>
    public ModelTemplate(GameObject prefab) => Prefab = prefab;
}
