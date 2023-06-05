using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    public UnityEvent EnterEvent;
    public UnityEvent CloseEvent; 

    public bool IsOpen = false;

    public CinemachineVirtualCamera gateCam;
    
    protected Button[] btns;
    protected TextMeshProUGUI[] texts;

    private Animator anim;
    public void Init() {
        gateCam = transform.Find("CMcam").GetComponent<CinemachineVirtualCamera>();
        btns = transform.Find("Canvas").GetComponentsInChildren<Button>();
        texts = transform.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
    }   
    public void SetOpen(bool value)
    {
        anim.SetBool("IsOpen",value);
        anim.SetTrigger("Change");
    }
    public void ExitGate()
    {
        Sequence seq = DOTween.Sequence();
        foreach(var b in btns)
        {
            seq.Insert(0,b.image.DOFade(0,0.5f));
        }
        foreach(var t in texts)
        {
            seq.Insert(0,t.DOFade(0,0.5f));
        }
        seq.OnComplete(()=>{
            seq.Kill();
        });
    }
    public void OpenAnimationEvent()
    {
        IsOpen = true;
    }
    public void SetCamPriority(int value)
    {
        gateCam.Priority = value;
    }

    public void EnterGate()
    {
        Sequence seq = DOTween.Sequence();
        foreach(var b in btns)
        {
            seq.Insert(0,b.image.DOFade(1,0.5f));
        }
        foreach(var t in texts)
        {
            seq.Insert(0,t.DOFade(1,0.5f));
        }
        seq.OnComplete(()=>{
            seq.Kill();
        });
        EnterEvent?.Invoke();
    }
    public void CloseAnimationEvent()
    {
        IsOpen = false;
        Sequence seq = DOTween.Sequence();
        foreach(var b in btns)
        {
            seq.Insert(0,b.image.DOFade(1,0.5f));
        }
        foreach(var t in texts)
        {
            seq.Insert(0,t.DOFade(1,0.5f));
        }
        seq.OnComplete(()=>{
            CloseEvent?.Invoke();
            seq.Kill();
        });
    }
    public void BtnActive(bool value)
    {
        foreach(var b in btns)
        {
            b.interactable = value;
        }
    }
}
