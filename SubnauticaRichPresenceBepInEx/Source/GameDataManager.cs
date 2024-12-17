using SubnauticaRichPresenceBepInEx.Source.Structs;
using SubnauticaRichPresenceBepInEx.Source.States;
using System;
using System.Collections.Generic;

namespace SubnauticaRichPresenceBepInEx.Source;

public static class GameDataManager
{
    #region Private Fields

    private static readonly Dictionary<string, BiomeData> _biomes = new();
    private static readonly Dictionary<string, VehicleData> _vehicles = new();
    private static readonly Dictionary<string, object> _datas = new();

    private static List<State> _states = new();

    #endregion

    public static void RegisterBiome(string id, BiomeData biomeData)
    {
        if (_biomes.ContainsKey(id)) return;
        _biomes.Add(id, biomeData);
    }

    public static void RegisterVehicle(string id, VehicleData vehicleData)
    {
        if (!_vehicles.ContainsKey(id)) return;
        _vehicles.Add(id, vehicleData);
    }

    public static void RegisterData(string id, object data)
    {
        if (!_datas.ContainsKey(id)) return;
        _datas.Add(id, data);
    }

    public static void RegisterState<TState>() where TState : State, new()
    {
        var state = new TState();
        state.OnCreate();

        _states.Add(state);
    }

    public static BiomeData GetBiomeData(string id)
    {
        if (_biomes.TryGetValue(id, out var biomeData))
        {
            return biomeData;
        }
        return default;
    }

    public static VehicleData GetVehicleData(string id)
    {
        if (_vehicles.TryGetValue(id, out var vehicleData))
        {
            return vehicleData;
        }
        return default;
    }

    public static TData GetData<TData>(string id)
    {
        if (_datas.TryGetValue(id, out var data))
        {
            return (TData)data;
        }
        return default;
    }

    internal static ref List<State> GetStates()
    {
        return ref _states;
    }
}
