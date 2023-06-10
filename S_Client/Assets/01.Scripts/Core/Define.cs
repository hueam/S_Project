using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Core
{
    
public class Define
{
    private Transform playerTrm = null;
    public Transform PlayerTrm {get{
        if(playerTrm = null)
        {
            playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
        }
        return playerTrm;
    }}
}   
public enum Managers
{
    PoolManager = 0,
    Client = 1,
    UIManager = 2,
}
public enum StateType
{
    Normal = 0,
}
}

