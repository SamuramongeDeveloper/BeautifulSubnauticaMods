using Baselib.Handlers;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baselib.Assets.Gadgets;

/// <summary>
/// Represents a base generator gadget.
/// </summary>
public sealed class BaseGeneratorGadget : Gadget
{
    /// <summary>
    /// Constructs a BaseGeneratorGadget.
    /// </summary>
    /// <param name="prefab">The custom prefab of this gadget.</param>
    public BaseGeneratorGadget(ICustomPrefab prefab) : base(prefab) { }

    protected override void Build()
    {
        var info = prefab.Info;
        if (CustomCellTypesHandler.TryAdd(info.ClassID, out Base.CellType result))
        {
            InternalLogger.Info($"Successfuly registered Base.CellType {result}.");
        }
        else InternalLogger.Error($"Failed to register Base.CellType {result}. ClassID empty or duplicate.");
    }
}
