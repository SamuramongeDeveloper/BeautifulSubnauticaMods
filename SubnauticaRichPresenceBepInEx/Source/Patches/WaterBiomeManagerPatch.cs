using HarmonyLib;
using System;
using System.Collections.Generic;
using SubnauticaRichPresenceBepInEx.Source.Structs;
using System.Text.RegularExpressions;

namespace SubnauticaRichPresenceBepInEx.Source.Patches;

[HarmonyPatch]
public static class WaterBiomeManagerPatch
{
    [HarmonyPatch(typeof(WaterBiomeManager), nameof(WaterBiomeManager.Start))]
    [HarmonyPostfix]
    private static void StartPostfix(WaterBiomeManager __instance)
    {
        var biomes = new Dictionary<string, BiomeData>();

        foreach (var kvp in __instance.biomeLookup)
        {
            var biomeId = kvp.Key;
            var biomeName = IdToName(biomeId);
            var biomeData = new BiomeData()
            {
                Client = 1073381647287853097,
                Pronoun = "The",
                Name = biomeName,
                Image = biomeId.ToLower()
            };

            biomes.Add(biomeId, biomeData);

            Logger.Log($"Biome added! {biomeId}, {biomeName}");
        }

        foreach (var kvp in biomes)
        {
            GameDataManager.RegisterBiome(kvp.Key, kvp.Value);
        }
    }

    private static string IdToName(string id)
    {
        var splitted = id.Split('_');

        var name = "";
        var match = Regex.Split(splitted[0], @"(?<!^)(?=[A-Z])");

        match.ForEach(str => { name += $"{str} "; });
        name = GetFirstCharUpper(name);

        return name;
    }

    // Probably better ways but meh, it works.
    private static string GetFirstCharUpper(string str)
    {
        var upperFirstChar = char.ToUpper(str[0]);
        var substring = str[1..];

        var result = upperFirstChar + substring;
        return result;
    }
}
