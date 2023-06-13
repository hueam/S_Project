using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.SceneManagement;

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

    private Transform playerTrm = null;
    public Transform PlayerTrm
    {
        get
        {
            if(playerTrm == null)
            {
                playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return playerTrm;
        }
    }

    public Dictionary<Managers,IManager> Managers = new Dictionary<Managers, IManager>();
    private IManager sceneController;
    public IManager SceneController{
        get 
        {
            if(sceneController == null)
            {
                Scene scene = SceneManager.GetActiveScene();
                sceneController = GameObject.Find($"{scene.name}Manager").GetComponent<IManager>();
                sceneController.Init(transform);
            }
            return sceneController;
        }
    }
    public bool IsLoad = false; 
    public SceneTypes SceneEnum = SceneTypes.Intro;
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
    public void LoadScene(int scnenNum)
    {
        StartCoroutine(LoadSceneCor(scnenNum));
        
    }
    private IEnumerator LoadSceneCor(int sceneNum)
    {
        IsLoad = true;
        AsyncOperation async = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneTypes),sceneNum));
        sceneController = null;
        while(!async.isDone)
        {
            yield return null;
        }
        IsLoad = false;
        SceneEnum = (SceneTypes)sceneNum;
        switch (SceneEnum)
        {
            case SceneTypes.Intro:
                {
                    break;
                }
            case SceneTypes.Room:
                {
                    ((RoomManager)SceneController).SpawnPlayer(true);
                    break;
                }
            case SceneTypes.InGame:
                {
                    GameObject obj = ((InGameManager)SceneController).SpawnPlayer(true);
                    ((Client)Managers[Core.Managers.Client]).SendData((int)Events.InGame,(int)InGameTypes.EnterP,JsonUtility.ToJson(new TransformPaket(obj.transform.position,obj.transform.rotation)));
                    break;
                }
        }
    }
}
