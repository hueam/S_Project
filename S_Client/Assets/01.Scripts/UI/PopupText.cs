using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : PoolableMono
{
    TextMeshProUGUI tmp;
    public override void Init()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        transform.position = new Vector3(500f,500f,transform.position.z);
    }
    public void SetMessage(string massage)
    {
        tmp.text = massage;
    }
}
