using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Baselib.Handlers;

namespace Baselib.Assets.Gadgets;

/// <summary>
/// Represents a base model gadget.
/// </summary>
public sealed class BaseModelGadget : Gadget
{
    /// <summary>
    /// Constructs a BaseModelGadget.
    /// </summary>
    /// <param name="prefab">The custom prefab of this gadget.</param>
    public BaseModelGadget(ICustomPrefab prefab) : base(prefab) { }

    protected override void Build()
    {
        var info = prefab.Info;
        if (BasePieceModelHandler.TryRegisterModel(info, out Base.Piece result))
        {
            InternalLogger.Info($"Successfully registered new model {result}.");
        }
        else InternalLogger.Error($"Failed to register new mod {result}. PrefabInfo empty or duplicate.");
    }
}
