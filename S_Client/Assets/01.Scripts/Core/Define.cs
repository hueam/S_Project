using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Core
{
    
public class Define
{
    private static AgentController player = null;
    public static AgentController Player {get{
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<AgentController>();
        }
        return player;
    }}
    private static Camera mainCam = null;
    public static Camera MainCam {
        get{
            if(mainCam == null)
            {
                mainCam = Camera.main;
            }
            return mainCam;
        }
    }
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

