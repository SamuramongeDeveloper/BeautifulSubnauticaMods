using Baselib.Assets.Gadgets;
using Baselib.Handlers;
using Baselib.Mono;
using Nautilus.Assets;
using Nautilus.Utility;
using System.Collections;
using UnityEngine;

namespace Baselib.Assets;

/// <summary>
/// Base class for defining new base pieces.
/// </summary>
public abstract class BasePiece
{
    private GhostTemplate _ghostTemplate;
    private ModelTemplate _modelTemplate;

    /// <summary>
    /// The information from the ghost generator prefab.
    /// </summary>
    public PrefabInfo Info { get; private set; }

    /// <summary>
    /// The CustomPrefab for the ghost generator of this base piece. Only registered after <seealso cref="Register"/>.
    /// </summary>
    public CustomPrefab Generator { get; private set; }

    /// <summary>
    /// The CustomPrefab for the base piece model. Only registered after <seealso cref="Register"/>.
    /// </summary>
    public CustomPrefab Model { get; private set; }

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

        Generator = new CustomPrefab(info);
        Generator.AddGadget(new BaseGeneratorGadget(Generator));
        Model = new CustomPrefab(modelInfo);
        Model.AddGadget(new BaseModelGadget(Model));
    }

    /// <summary>
    /// Registers this base piece into the game.
    /// </summary>
    public void Register()
    {
        _ghostTemplate = CreateGhostGenerator();
        _modelTemplate = CreateModel();

        if (Info.TechType == TechType.None)
        {
            InternalLogger.Error("Attempt to register a base piece with an invalid TechType halted. Returning early.");
            return;
        }

        Generator.SetGameObject(GetGhostGeneratorPrefab);
        Generator.Register();

        Model.SetGameObject(GetModelPrefab);
        Model.Register();
    }

    /// <summary>
    /// Overrideable method that applies the necessary materials to the base piece model.
    /// </summary>
    /// <param name="model">The model to apply them materials to.</param>
    public virtual void ApplyMaterialsToModel(GameObject model)
    {
        MaterialUtils.ApplySNShaders(model);
    }

    /// <summary>
    /// Method to modify the base piece model futher more to your necessities.
    /// </summary>
    /// <param name="model">The model to modify.</param>
    /// <returns>A coroutine.</returns>
    protected abstract IEnumerator ModifyModel(GameObject model);

    /// <summary>
    /// Expects a non-null <seealso cref="GhostTemplate"/>.
    /// </summary>
    /// <returns>A GhostTemplate for this base piece.</returns>
    protected abstract GhostTemplate CreateGhostGenerator();

    /// <summary>
    /// Expects a non-null <seealso cref="ModelTemplate"/>.
    /// </summary>
    /// <returns>A model for this base piece.</returns>
    protected abstract ModelTemplate CreateModel();

    private void ApplyGeneratorComponents(GameObject prefab)
    {
        var baseGhost = prefab.transform.Find("BaseGhost");
        if (baseGhost == null)
        {
            InternalLogger.Error("Unable to apply components to generator prefab. Invalid prefab.");
            return;
        }

        var constructableData = _ghostTemplate.MonoConstructable;
        var constructableBase = PrefabUtils.AddConstructable<ConstructableBase>(prefab, Info.TechType, constructableData.ConstructableFlags, baseGhost.gameObject);
        constructableBase.placeMinDistance = constructableData.PlacingMinDistance;
        constructableBase.placeMaxDistance = constructableData.PlacingMaxDistance;
        constructableBase.placeDefaultDistance = constructableData.PlacingDefaultDistance;
        constructableBase.rotatableBasePiece = constructableData.ConstructableFlags.HasFlag(ConstructableFlags.Rotatable);
        constructableBase.attachedToBase = constructableData.AttachedToBase;
        constructableBase.builtBoxFX = BuildingReferences.BuiltBoxFX;
        constructableBase._EmissiveTex = BuildingReferences.EmissiveTexture;
        constructableBase._NoiseTex = BuildingReferences.NoiseTexture;

        var baseGhostData = _ghostTemplate.MonoGhostData;
        var baseGhostComponent = baseGhost.gameObject.AddComponent<CustomBaseGhost>();
        baseGhostComponent.allowedAboveWater = baseGhostData.AllowedAboveWater;
        CustomCellTypesHandler.TryGet(Info.ClassID, out var cellType);
        baseGhostComponent.CellType = cellType;

    }

    private IEnumerator GetGhostGeneratorPrefab(IOut<GameObject> prefab)
    {
        var prefabCopy = UWE.Utils.InstantiateDeactivated(_ghostTemplate.Prefab);
        ApplyGeneratorComponents(prefabCopy);
        prefab.Set(prefabCopy);
        yield break;
    }

    private IEnumerator GetModelPrefab(IOut<GameObject> prefab)
    {
        var prefabCopy = UWE.Utils.InstantiateDeactivated(_modelTemplate.Prefab);
        yield return ModifyModel(prefabCopy);
        ApplyMaterialsToModel(prefabCopy);
        prefab.Set(prefabCopy);
        yield break;
    }
}
