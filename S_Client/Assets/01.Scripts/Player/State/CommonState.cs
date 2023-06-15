using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonState : MonoBehaviour, IState
{
    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract bool UpdateState();

    protected AgentAnimator _animator;
    protected PlayerInput _agentInput;
    protected AgentController _agentController;
    protected PlayerMovement _agentMovement; //�̵�����

    public virtual void SetUp(Transform agentRoot)
    {
        _animator = GameObject.Find("OverLayCam/Arm").GetComponent<AgentAnimator>();
        _agentInput = agentRoot.GetComponent<PlayerInput>();
        _agentController = agentRoot.GetComponent<AgentController>();
        _agentMovement = agentRoot.GetComponent<PlayerMovement>();
    }

    //�ǰ�ó���� ����� ��
    public void OnHitHandle(Vector3 hitPoint, Vector3 Normal)
    {

    }
}
