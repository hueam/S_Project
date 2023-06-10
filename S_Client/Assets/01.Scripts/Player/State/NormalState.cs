using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : CommonState
{
    //protected AgentMovement _agentMovement;

    public override void OnEnterState()
    {
        _agentMovement?.StopImmediately();
        _agentInput.movementInput += OnMovementHandle;
        _agentInput.mouseMove += OnMouseMovehandle;
        _agentInput.jumpKeyPress += OnJumpHandle;
    }


    public override void OnExitState()
    {
        _agentMovement?.StopImmediately();
        _agentInput.movementInput -= OnMovementHandle;
        _agentInput.mouseMove -= OnMouseMovehandle;
        _agentInput.jumpKeyPress -= OnJumpHandle;
    }

    private void OnJumpHandle()
    {
        _agentMovement?.Jump();
    }

    private void OnMovementHandle(Vector3 obj)
    {
        _agentMovement?.SetMovementVelocity(obj);
    }
    private void OnMouseMovehandle(Vector2 obj)
    {
        _agentMovement?.PlayerRotate(obj);
    }

    //public override void SetUp(Transform agentRoot)
    //{
    //    base.SetUp(agentRoot);
    //    _agentMovement = agentRoot.GetComponent<AgentMovement>();
    //}
     
    public override bool UpdateState()
    {
        return true;
    }

}
