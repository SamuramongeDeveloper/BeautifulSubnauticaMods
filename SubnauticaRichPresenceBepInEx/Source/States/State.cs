namespace SubnauticaRichPresenceBepInEx.Source.States;

public abstract class State
{
    public abstract bool CanUseState();

    public virtual void OnCreate() { }

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }
}
