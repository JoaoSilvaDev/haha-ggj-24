public class StateResults : State
{
    public override void OnEnterState()
    {
        SoundManager.instance.PlaySound("result-screen");
        base.OnEnterState();
    }

    public override void OnExitState()
    {
        SoundManager.instance.StopSound("result-screen");
        base.OnExitState();
    }
}