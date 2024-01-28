public class StateTitleScreen : State
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        SoundManager.instance.PlaySound("sleep", true);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        SoundManager.instance.StopSound("sleep");
        SoundManager.instance.PlaySound("fart");
    }
}