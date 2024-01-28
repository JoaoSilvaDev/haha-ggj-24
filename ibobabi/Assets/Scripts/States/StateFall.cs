public class StateFall : State
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        SoundManager.instance.PlaySound("hurt");
        SoundManager.instance.PlaySound("fall");
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}