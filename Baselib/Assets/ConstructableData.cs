using Nautilus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Baselib.Assets;

/// <summary>
/// Contains data to set up a <seealso cref="ConstructableBase"/>.
/// </summary>
public struct ConstructableData
{
    /// <summary>
    /// Defines the data pertaining on how the <seealso cref="ConstructableBase"/> should behave.
    /// </summary>
    public ConstructableFlags ConstructableFlags {  get; set; }

    /// <summary>
    /// Defines if this constructable is attached to a base.
    /// <para>Now I know this sounds stupid for BaseLib, but who knows what you want to do with this lib, alright?</para>
    /// </summary>
    public bool AttachedToBase { get; set; }

    /// <summary>
    /// The maximum ammount of distance where the constructable can be placed.
    /// </summary>
    public float PlacingMaxDistance { get; set; }

    /// <summary>
    /// The minimum ammount of distance where the constructable can be placed.
    /// </summary>
    public float PlacingMinDistance { get; set; }

    /// <summary>
    /// The default distance where constructable can be placed if the player surpass the limits (min/max).
    /// </summary>
    public float PlacingDefaultDistance { get; set; }
}
