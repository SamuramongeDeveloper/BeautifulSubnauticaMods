using JetBrains.Annotations;
using SubnauticaRichPresenceBepInEx.Source.States;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace SubnauticaRichPresenceBepInEx.Source;

public class DiscordMono : MonoBehaviour
{
    #region Private Fields

    private List<State> _states;
    private State _currentState;

    private Timer _timer;

    #endregion

    private void Start()
    {
        _timer = new(4000); // 4 seconds.
        _timer.Elapsed += (s, e) => { Discord.Update(); };
        _timer.Start();

        // Using ref so we don't need to detect changes in the list.
        _states = GameDataManager.GetStates();
    }

    private void Update()
    {
        _currentState = _states.Find(Match);
        _currentState?.OnUpdate();
    }

    private void OnApplicationQuit()
    {
        _timer.Stop();
        Discord.Dispose();
    }

    private bool Match(State state)
    {
        if (state.CanUseState()) state.OnStart();
        return false;
    }
}
