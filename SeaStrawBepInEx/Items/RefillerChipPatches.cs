using HarmonyLib;

namespace SeaStraw.Items;

[HarmonyPatch]
public class RefillerChipPatches
{
    [HarmonyPatch(typeof(Player), nameof(Player.Awake))]
    [HarmonyPrefix]
    private static void PlayerAwakePrefix(Player __instance)
    {
        __instance.gameObject.EnsureComponent<Refiller>();
    }
}
