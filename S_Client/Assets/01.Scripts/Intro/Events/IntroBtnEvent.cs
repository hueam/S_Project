using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class IntroBtnEvent : MonoBehaviour,IEvent
{
    [SerializeField]
    UnityEvent dissolveEndEvnet = null;
    [SerializeField]List<Button> btns = null;
    [SerializeField]List<TextMeshProUGUI> texts = null;
    private void Start() {
        transform.parent.GetComponentsInChildren<Button>(btns);
        transform.parent.GetComponentsInChildren<TextMeshProUGUI>(texts);
    }
    public void Evnet()
    {
        Sequence seq = DOTween.Sequence();
        btns.ForEach(b =>
        {
            seq.Insert(0,b.image.DOFade(0,0.5f));
        });
        texts.ForEach(t =>
        {
            seq.Insert(0,t.DOFade(0,0.5f));
        });
        seq.OnComplete(()=>dissolveEndEvnet?.Invoke());
    }
} 