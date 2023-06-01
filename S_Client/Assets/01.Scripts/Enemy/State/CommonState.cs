using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonState : MonoBehaviour, IState
{
    public abstract void OnEnterState();

    public abstract void OnExitState();
    public abstract void UpdateState();

    public virtual void SetUp(Transform agentRoot)
    {

    }

}
