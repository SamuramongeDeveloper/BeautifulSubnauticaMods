using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using UnityEngine;
using Ingredient = CraftData.Ingredient;


namespace SeaStraw.Items;

public class RefillerChipPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("refillerchip", "Refiller Chip", "Automatically fills every SeaStraw in your inventory.", unlockAtStart: false)
        .WithIcon(SpriteManager.Get(TechType.ComputerChip));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        var @object = new CloneTemplate(Info, TechType.MapRoomHUDChip);

        prefab.SetRecipe(new RecipeData()
        {
            craftAmount = 1,
            Ingredients = 
            {
                new Ingredient(TechType.ComputerChip),
                new Ingredient(TechType.AdvancedWiringKit),
                new Ingredient(TechType.Titanium, 3),
                new Ingredient(TechType.Gold, 3)
            }
        })
            .WithStepsToFabricatorTab("Personal", "Equipment")
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithCraftingTime(1.1f);
        prefab.SetUnlock(TechType.ComputerChip)
            .WithPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
        prefab.SetEquipment(EquipmentType.Chip);

        prefab.SetGameObject(@object);
        prefab.Register();
    }
}

public class Refiller : MonoBehaviour
{
    public static Refiller Main { get; private set; }

    public List<Straw> Straws { get; private set; }

    private List<string> slots;
    private Equipment equipment;

    private void Start()
    {
        Straws = new();
        slots = new();
        equipment = Inventory.main.equipment;
        equipment.GetSlots(EquipmentType.Chip, slots);
        Main = this;
    }

    private void Update()
    {
        if (!GetEquipped()) return;

        foreach (var straw in Straws)
        {
            if (straw is null) // The straw gameobject was destroyed and so the straw.
            {
                Straws.Remove(straw);
            }
            else
            {
                straw.RefillSlow();
            }
        }
    }

    private bool GetEquipped()
    {
        foreach (var slot in slots)
        {
            if (equipment.GetTechTypeInSlot(slot) == RefillerChipPrefab.Info.TechType)
            {
                return true;
            }
        }
        return false;
    }
}