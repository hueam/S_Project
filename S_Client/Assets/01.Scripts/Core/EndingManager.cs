using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour, IManager
{
    [SerializeField]
    TextMeshProUGUI text;
    public void Init(Transform parent)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void SetWin(bool IsWin){
        text.text = IsWin?"Win!":"Lose..";
    }
    public void GotoIntro()
    {
        ((Client)GameManager.Instance.Managers[Core.Managers.Client]).SendData((int)Events.Ending,(int)EndingTypes.Goto,"");
    }
}
