using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using UnityEngine;
using Ingredient = CraftData.Ingredient;

namespace SeaStraw.Items;

public class SeaStrawPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("seastraw", "SeaStraw", "Purifies water around you for consumption.")
        .WithIcon(SpriteManager.Get(TechType.DisinfectedWater));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        var @object = new CloneTemplate(Info, TechType.DisinfectedWater);

        @object.ModifyPrefab += prefab =>
        {
            if (prefab.TryGetComponent<PlayerTool>(out var tool))
            {
                Object.DestroyImmediate(tool);
            }

            prefab.EnsureComponent<Straw>();
            prefab.EnsureComponent<StrawTool>();
        };

        prefab.SetRecipe(new RecipeData() 
        {
            craftAmount = 1,
            Ingredients =
            {
                new Ingredient(TechType.DisinfectedWater),
                new Ingredient(TechType.ComputerChip),
                new Ingredient(TechType.WiringKit)
            }
        })
            .WithStepsToFabricatorTab("Survival", "Water")
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithCraftingTime(1.5f);
        prefab.SetUnlock(TechType.ComputerChip)
            .WithPdaGroupCategory(TechGroup.Survival, TechCategory.Water);
        prefab.SetEquipment(EquipmentType.Hand)
            .WithQuickSlotType(QuickSlotType.Selectable);

        prefab.SetGameObject(@object);
        prefab.Register();
    }
}

public class Straw : MonoBehaviour
{
    private Eatable eatable;
    private Rigidbody rigidbody;

    public float waterValue;
    public float waterPercentage;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        eatable = GetComponent<Eatable>();
        eatable.waterValue = 0;

        waterValue = 0;
        waterPercentage = 0;

        Refiller.Main.Straws.Add(this);
    }

    private void OnEnable()
    {
        rigidbody.detectCollisions = !transform.parent;
    }

    private void OnDisable()
    {
        rigidbody.detectCollisions = false;
    }

    public void Refresh()
    {
        waterPercentage = waterValue < 1 ? 0 : (waterValue / 100);
        eatable.waterValue = waterValue;
    }

    public void Refill()
    {
        if (Player.main.IsUnderwater())
        {
            waterValue = Mathf.Clamp(waterValue + SeaStraw.Config.SeaStrawRegenerationRate * Time.deltaTime, 0f, 100f);
        }
        Refresh();
    }

    public void RefillSlow()
    {
        if (Player.main.IsUnderwater())
        {
            waterValue = Mathf.Clamp(waterValue + SeaStraw.Config.RefillerChipRegenerationRate * Time.deltaTime, 0f, 100f);
        }
        Refresh();
    }

    public float GetWaterToDrink()
    {
        return waterValue <= SeaStraw.Config.Consume ? waterValue : SeaStraw.Config.Consume;
    }
}

public class StrawTool : PlayerTool
{
    private Straw straw;
    private Survival survival;

    public override string animToolName => TechType.Knife.ToString().ToLower();

    public override void Awake()
    {
        base.Awake();
        straw = GetComponent<Straw>();
        survival = Player.main.GetComponent<Survival>();
    }

    public override bool OnRightHandHeld()
    {
        straw.Refill();
        return false;
    }

    public override bool OnRightHandDown()
    {
        if (!Player.main.IsUnderwater())
        {
            survival.Eat(straw.gameObject);
        }
        return false;
    }
}