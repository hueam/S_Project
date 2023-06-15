using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour,IManager
{
    [SerializeField]GameObject playerPrefab,otherPlayerPreb;
    [SerializeField]List<Transform> spawnTrms = new List<Transform>();
    [SerializeField]TextMeshProUGUI bulletText;


    public void Init(Transform parent)
    {
        
    }
    public GameObject SpawnPlayer(bool isMy = true)
    {
        return Instantiate(isMy ? playerPrefab : otherPlayerPreb, spawnTrms[Random.Range(0,spawnTrms.Count)].position, Quaternion.identity);
    }
    public void SetBulletText(int currentAmmo, int maxAmmo)
    {
        bulletText.text = $"{currentAmmo}/{maxAmmo}";
    }

}
