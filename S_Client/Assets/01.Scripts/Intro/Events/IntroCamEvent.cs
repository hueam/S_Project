using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class IntroCamEvent : MonoBehaviour,IEvent
{
    CinemachineVirtualCamera thisCam;
    CinemachineVirtualCamera[] cams;

    private void Start() {
        thisCam = GetComponent<CinemachineVirtualCamera>();
        cams = FindObjectsOfType<CinemachineVirtualCamera>();
    }
    public void Evnet(){
        StartCoroutine(DelayOpen());
    }
    IEnumerator DelayOpen()
    {
        yield return new WaitForSeconds(1f);
        foreach(var c in cams)
        {
            c.Priority = 5;
        }
        thisCam.Priority = 10;
    }
}
