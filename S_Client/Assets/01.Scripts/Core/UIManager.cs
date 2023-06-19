using System.Collections;
using System.Collections.Generic;
using Core;
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
        RectTransform rectTrm = userUI.GetComponent<RectTransform>();
        text.SetMessage(massage,rectTrm.TransformPoint(rectTrm.rect.center));
    }
    public void SetDiePanel(bool b)
    {
        userUI.transform.Find("Die").gameObject.SetActive(b);
    }
    
}
