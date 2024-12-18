using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Baselib.Assets;

/// <summary>
/// Contains data to set up a <seealso cref="BaseGhost"/>.
/// </summary>
public readonly struct BaseGhostData
{
    /// <summary>
    /// If the base ghost is allowed to be above water.
    /// <para>I.E Moonpools aren't allowed to be above water.</para>
    /// </summary>
    public bool AllowedAboveWater { get; }

    /// <summary>
    /// Constructs a new BaseGhost data object.
    /// </summary>
    /// <param name="alloweAboveWater">If the base is allowed to be above water.</param>
    public BaseGhostData(bool alloweAboveWater)
    {
        AllowedAboveWater = alloweAboveWater;
    }
}
