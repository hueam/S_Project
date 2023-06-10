using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Core;

public class RoomBtn : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField]bool Create;
    public void SendServer()
    {
        if(Create == true)
            ((Client)GameManager.Instance.Managers[Managers.Client]).CreateRoom(inputField.text);
        else
            ((Client)GameManager.Instance.Managers[Managers.Client]).JoinRoom(inputField.text);

        inputField.text = null;
    }
}
