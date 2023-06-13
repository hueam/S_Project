using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour,IManager
{
    [SerializeField]GameObject playerPrefab,otherPlayerPreb;
    [SerializeField]List<Transform> spawnTrms = new List<Transform>();
    public void Init(Transform parent)
    {
        
    }
    public GameObject SpawnPlayer(bool isMy = true)
    {
        return Instantiate(isMy ? playerPrefab : otherPlayerPreb, spawnTrms[Random.Range(0,spawnTrms.Count)].position, Quaternion.identity);
    }

}
