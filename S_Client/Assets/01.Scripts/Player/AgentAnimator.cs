using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : MonoBehaviour
{
    private readonly int speedHash = Animator.StringToHash("Speed"); 
    private readonly int inputXHash = Animator.StringToHash("InputX"); 
    private readonly int inputYHash = Animator.StringToHash("InputY"); 
    private readonly int reloading = Animator.StringToHash("Reloading");
    private readonly int reload = Animator.StringToHash("Reload");

    public event Action animationEvnet;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void SetTriggerReload(bool value)
    {
        if(value)
            animator.SetTrigger(reload);
        else
            animator.ResetTrigger(reload);
    }
    public void SetBoolReload(bool value)
    {
        animator.SetBool(reloading,value);
    }
    public void SetFloatSpeed(float speed)
    {
        animator.SetFloat(speedHash,speed);
    }
    public void SetFloatInputX(float inputX)
    {
        animator.SetFloat(inputXHash,inputX);
    }
    public void SetFloatInputY(float inputY)
    {
        animator.SetFloat(inputYHash,inputY);
    }
    public void InvokeAnimationEvent()
    {
        animationEvnet?.Invoke();
    }
}
