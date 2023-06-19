using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class DieState : CommonState
{
    Transform agentTrans;
    float timer = 0;
    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        agentTrans = agentRoot;

    }
    public override void OnEnterState()
    {
        _agentMovement?.StopImmediately();

        _agentInput.movementInput += MovementHandle;
        _agentInput.mouseMove += OnMouseMovehandle;
        ((Client)GameManager.Instance.Managers[Managers.Client]).alwayEvnet -= _agentMovement.SendTransform;
        ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).SetDiePanel(true);
        _agentController.IsDead = true;
        timer = 0f;
    }

    public override void OnExitState()
    {
        _agentMovement?.StopImmediately();

        _agentController.IsDead = false;
        _agentInput.movementInput -= MovementHandle;
        _agentInput.mouseMove -= OnMouseMovehandle;
        ((InGameManager)GameManager.Instance.SceneController).ReSpawn(agentTrans.gameObject);
        ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).SetDiePanel(false);
        ((Client)GameManager.Instance.Managers[Managers.Client]).alwayEvnet += _agentMovement.SendTransform;
    }

    public override bool UpdateState()
    {
        timer += Time.deltaTime;
        if(timer > 10f)
        {
            _agentController.ChangeState(Core.StateType.Normal);
        }
        return true;
    }
    private void OnMouseMovehandle(Vector2 obj)
    {
        _agentMovement?.PlayerRotate(obj);
    }
    private void MovementHandle(Vector3 vector)
    {
        _agentMovement.SetMovementVelocity(vector);
    }
}
