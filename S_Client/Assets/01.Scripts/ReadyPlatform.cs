using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class ReadyPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log(11);
            
        }
    }
    private void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Player"))
        {
            ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.Room,(int)RoomTypes.Ready,JsonUtility.ToJson(false));
        }
    }
}
