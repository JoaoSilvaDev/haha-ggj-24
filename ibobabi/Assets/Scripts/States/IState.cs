public interface IState
{
    void OnEnterState();
    void OnExitState();
    void UpdateState();
}