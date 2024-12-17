using UnityEngine.SceneManagement;

namespace SubnauticaRichPresenceBepInEx.Source.States;

public class MenuState : State
{
    private bool canUse;

    public override bool CanUseState() => canUse;

    public override void OnCreate()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnStart()
    {
        Discord.Activity = new Activity()
        {
            Name = "On Main Menu.",
            Type = ActivityType.Playing,
        };
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name == "XMenu")
        {
            canUse = true;
        }
        canUse = false;
    }
}
