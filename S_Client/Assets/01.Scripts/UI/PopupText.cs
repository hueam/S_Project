using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PopupText : PoolableMono
{
    TextMeshProUGUI tmp;
    public override void Init()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    public void SetMessage(string massage, Vector3 pos)
    {
        tmp.text = massage;
        transform.position = pos;
        tmp.DOFade(0,1f).SetEase(Ease.InSine);
        tmp.transform.DOMove(new Vector3(pos.x,pos.y+50f,pos.z), 1f).SetEase(Ease.InSine);
    }
}
