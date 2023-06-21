using Cinemachine;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : CommonState
{
    private bool IsReLoad;
    public CinemachineVirtualCamera vCam;

    public override void OnEnterState()
    {
        _agentMovement?.StopImmediately();
        _agentInput.movementInput += OnMovementHandle;
        _agentInput.mouseMove += OnMouseMovehandle;
        _agentInput.jumpKeyPress += OnJumpHandle;
        _agentInput.fireKeyPress += OnFireHandle;
        _agentInput.ReloadKeyPresss += OnReloadHandle;
        _animator.animationEvnet += AnimationEventTrigger;
        _agentInput.ZoomKeyPress += OnZoomHandle;
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
        _agentInput.ZoomKeyPress -= OnZoomHandle;
    }
    private void OnReloadHandle()
    {
        if(GameManager.Instance.SceneEnum == SceneTypes.InGame)
        {
            _animator.SetTriggerReload(true);
            _animator.SetBoolReload(true);
            IsReLoad = true;
        }
    }
    private void AnimationEventTrigger()
    {
        _animator.SetBoolReload(false);
        _agentController.currentGun.Reload();
    }

    private void OnFireHandle()
    {
        if(GameManager.Instance.SceneEnum == SceneTypes.InGame)
        {
            if (IsReLoad == true)
            {
                _animator.SetBoolReload(false);
                IsReLoad = false;
            }
            else if (IsReLoad == false)
            {
                if (_agentController.currentGun.Fire() == false)
                {
                    IsReLoad = true;
                    _animator.SetTriggerReload(true);
                    _animator.SetBoolReload(true);
                }
            }
        }
    }
    float percent = 0;
    private void OnZoomHandle(bool isPress)
    {
        percent += isPress ? Time.deltaTime*5 : -Time.deltaTime*5;
        percent = Mathf.Clamp(percent, 0, 1);
        _agentController.overlayCam.transform.localPosition = Vector3.Lerp(Vector3.zero, _agentController.CamZoomTrm.localPosition,percent);
        _agentController.overlayCam.transform.localRotation = Quaternion.Lerp(Quaternion.identity, _agentController.CamZoomTrm.localRotation,percent);
        _agentController.overlayCam.fieldOfView = Mathf.Lerp(60f,25f,percent);
        vCam.m_Lens.FieldOfView = Mathf.Lerp(60f,25f,percent);

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
