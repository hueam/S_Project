using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class UIManager : MonoBehaviour,IManager
{
    Canvas userUI;
    public void Init(Transform parent)
    {
        SetMainUi();
    }
    public void SetMainUi()
    {
        userUI = GameObject.Find("MainCanvas").GetComponent<Canvas>();
    }
    public void ShowMessage(string massage)
    {
        PopupText text = ((PoolManager)GameManager.Instance.Managers[Managers.PoolManager]).Pop("PopUpText") as PopupText;
        text.transform.parent = userUI.transform;
        text.SetMessage(massage);
    }
    
}
