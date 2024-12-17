using HarmonyLib;
using System;
using UnityEngine;

namespace SeaStraw.Items;

[HarmonyPatch]
public class SeaStrawPatches
{
    [HarmonyPatch(typeof(Survival), nameof(Survival.Eat))]
    [HarmonyPrefix]
    private static bool SurvivalEatPrefix(Survival __instance, GameObject useObj)
    {
        if (useObj.TryGetComponent<Straw>(out var straw))
        {
            var waterToDrink = straw.GetWaterToDrink();
            var consume = (waterToDrink + __instance.water) >= 100 ? (100 - __instance.water) : waterToDrink;

            straw.waterValue -= consume;
            straw.Refresh();

            if (__instance.water < 20f) __instance.vitalsOkNotification.Play();

            __instance.water = Mathf.Clamp(__instance.water + waterToDrink, 0f, 100f);
            __instance.onDrink.Trigger(waterToDrink);
            GoalManager.main.OnCustomGoalEvent("Drink_Something");
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(TooltipFactory), nameof(TooltipFactory.GetBarValue))]
    [HarmonyPatch(new Type[] { typeof(Pickupable) })]
    [HarmonyPostfix]
    private static void TooltipFactoryGetBarValuePostfix(ref float __result, Pickupable pickupable)
    {
        if (pickupable.TryGetComponent<Straw>(out var straw))
        {
            straw.Refresh();
            __result = straw.waterPercentage;
        }
    }
}
