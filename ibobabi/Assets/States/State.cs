using System;
using UnityEngine;

public abstract class State : IState
{
    public Action OnEnter;
    public Action OnExit;

    public virtual void OnEnterState()
    {
        //Debug.Log($"Entering {GetType().Name} State");
        OnEnter?.Invoke();
    }

    public virtual void OnExitState()
    {
        //Debug.Log($"Exiting {GetType().Name} State");
        OnExit?.Invoke();
    }

    public virtual void UpdateState()
    {
        //Debug.Log($"Update {GetType().Name} State");
    }
}