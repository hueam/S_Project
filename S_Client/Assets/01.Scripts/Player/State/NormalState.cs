using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : CommonState
{
    private bool IsReLoad;

    public override void OnEnterState()
    {
        _agentMovement?.StopImmediately();
        _agentInput.movementInput += OnMovementHandle;
        _agentInput.mouseMove += OnMouseMovehandle;
        _agentInput.jumpKeyPress += OnJumpHandle;
        _agentInput.fireKeyPress += OnFireHandle;
        _agentInput.ReloadKeyPresss += OnReloadHandle;
        _animator.animationEvnet += AnimationEventTrigger;
    }


    public override void OnExitState()
    {
        _agentMovement?.StopImmediately();
        _agentInput.movementInput -= OnMovementHandle;
        _agentInput.mouseMove -= OnMouseMovehandle;
        _agentInput.jumpKeyPress -= OnJumpHandle;
        _agentInput.fireKeyPress -= OnFireHandle;
        _agentInput.ReloadKeyPresss -= OnReloadHandle;
        _animator.animationEvnet -= AnimationEventTrigger;
    }
    private void OnReloadHandle()
    {
         _animator.SetTriggerReload(true);
        _animator.SetBoolReload(true);
        IsReLoad = true;
    }
    private void AnimationEventTrigger()
    {
        _animator.SetBoolReload(false);
        _agentController.currentGun.Reload();
    }

    private void OnFireHandle()
    {
        if(IsReLoad == true){
            _animator.SetBoolReload(false);
            IsReLoad = false;
        }    
        else if(IsReLoad == false)
        {
            if(_agentController.currentGun.Fire() == false) 
            {
                IsReLoad = true;
                _animator.SetTriggerReload(true);
                _animator.SetBoolReload(true);
            }
        }
       
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
