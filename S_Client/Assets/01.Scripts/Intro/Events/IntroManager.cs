using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    List<Gate> gates = new List<Gate>();

    Gate currentGate = null;
    int currentIdx = 0;
    private void Awake() {
        foreach(var gate in gates)
        {
            gate.Init();
        }
        currentIdx = 1;
        currentGate = gates[currentIdx];
        currentGate.EnterGate();
    }

    public void NextGate()
    {
        currentIdx++;
        currentGate?.SetOpen(true);

        StartCoroutine(ChangeGate(currentIdx));
    }
    public void PrevGate()
    {
        currentIdx--;
        StartCoroutine(ChangeGate(currentIdx,true));
        
    }

    private IEnumerator DelayCor(float delay = 1,Action callback = null)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    private IEnumerator ChangeGate(int idx,bool IsPrev = false)
    {
        currentGate?.ExitGate();
        if(IsPrev == false)yield return new WaitUntil(()=>(currentGate?.IsOpen == true));
        currentGate?.SetCamPriority(5);
        currentGate = gates[idx];
        currentGate?.SetCamPriority(10);
        if(IsPrev == true) yield return DelayCor(2f,()=>{
            currentGate.SetOpen(false);
            currentGate?.EnterGate();
        });
        else currentGate?.EnterGate();
    }
    public void GameEnd()
    {
        StartCoroutine(DelayCor(1f,Application.Quit));
    }
}
