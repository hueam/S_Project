using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {get{
        if(instance == null)
        {
            instance = FindObjectOfType<GameManager>();
        }
        return instance;
    }}
    public Dictionary<Managers,IManager> Managers = new Dictionary<Managers, IManager>();

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        int managerCount = Enum.GetNames(typeof(Managers)).Length;
        for(int i = 0; i < managerCount; i++)
        {
            IManager manager = transform.Find(((Managers)i).ToString()).GetComponent<IManager>();
            manager.Init(transform);
            Managers.Add((Managers)i,manager);
        }
    }
}
