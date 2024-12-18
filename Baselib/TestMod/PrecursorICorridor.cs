using Baselib.Assets;
using Nautilus.Utility;
using Nautilus.Assets;
using Nautilus.Crafting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nautilus.Assets.Gadgets;

namespace Baselib.TestMod;

public sealed class PrecursorICorridor : BasePiece
{
    public PrecursorICorridor(PrefabInfo info) : base(info)
    {
        Generator.SetRecipe(new RecipeData()
        {
            craftAmount = 1,
            Ingredients = new List<CraftData.Ingredient>
            {
                new(TechType.TitaniumIngot, 2),
                new(TechType.PrecursorIonCrystal)
            }
        });
        Generator.SetPdaGroupCategory(TechGroup.BasePieces, TechCategory.BaseRoom);
    }

    protected override GhostTemplate CreateGhostGenerator()
    {
        return new GhostTemplate(GhostTemplate.GhostGeneratorTemplate)
        {
            MonoConstructable = new ConstructableData()
            {
                ConstructableFlags = ConstructableFlags.Rotatable | ConstructableFlags.Outside,
                AttachedToBase = true,
                PlacingMinDistance = 2,
                PlacingMaxDistance = 10,
                PlacingDefaultDistance = 6,
            },
            MonoGhostData = new BaseGhostData(true),
        };
    }

    protected override ModelTemplate CreateModel()
    {
        var asset = Plugin.PrecursorBasesBundle.LoadAsset<GameObject>("assets/baselib/PrecursorICorridorShape.prefab");
        return new ModelTemplate(asset);
    }

    protected override IEnumerator ModifyModel(GameObject model)
    {
        yield break;
    }
}
