using Baselib.Handlers;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Baselib.Assets;

// ECCLibrary remember.

/// <summary>
/// Base class for defining new base pieces.
/// </summary>
public abstract class BasePiece
{
    /// <summary>
    /// The information from the ghost generator prefab.
    /// </summary>
    public PrefabInfo Info { get; private set; }

    /// <summary>
    /// The CustomPrefab for the ghost generator of this base piece. Only registered after <seealso cref="Register"/>.
    /// </summary>
    public CustomPrefab PrefabGenerator { get; private set; }

    /// <summary>
    /// The CustomPrefab for the base piece model. Only registered after <seealso cref="Register"/>.
    /// </summary>
    public CustomPrefab PrefabModel { get; private set; }

    /// <summary>
    /// Constructs a base piece with the specified prefab information.
    /// <para>The model info is automatically created.</para>
    /// </summary>
    /// <param name="info">The required information to register the prefab.</param>
    public BasePiece(PrefabInfo info)
    {
        
        Info = info;
        var shapeName = Info.ClassID + "baseshape";
        var modelInfo = PrefabInfo.WithTechType(shapeName);

        PrefabGenerator = new CustomPrefab(info);
        PrefabModel = new CustomPrefab(modelInfo);
    }

    /// <summary>
    /// Registers this base piece into the game,
    /// </summary>
    public void Register()
    {

    }

    protected abstract void CreateGenerator();

    protected abstract void CreateModel();
}
