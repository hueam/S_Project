using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPInput : MonoBehaviour
{
    public static string IP;

    public Button btn;

    public void ChangeIPField(string str)
    {
        IP = str;
        btn.gameObject.SetActive(str.Length > 0);
    }
}
