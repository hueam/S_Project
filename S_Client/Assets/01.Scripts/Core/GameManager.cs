using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string roomID;
    private static GameManager instance;
    public static GameManager Instance {get{
        if(instance == null)
        {
            instance = FindObjectOfType<GameManager>();
        }
        return instance;
    }}


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

    private int winPoint = 0;
    public int WinPoint{
        get => winPoint;
        set {
            if(SceneEnum == SceneTypes.InGame)
            {
                winPoint = value;
                ((InGameManager)SceneController).SetScoreText(winPoint);
                if(winPoint == 20)
                    ((Client)Managers[Core.Managers.Client]).SendData((int)Events.InGame,(int)InGameTypes.Ending,"");
            }
        }
    }
    private void Awake() {
        if(instance != null) Destroy(this.gameObject);
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
        ((Client)Managers[Core.Managers.Client]).envetClear();
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
                    AgentController a = ((RoomManager)SceneController).SpawnPlayer(true).GetComponent<AgentController>();
                    break;
                }
            case SceneTypes.InGame:
                {
                    GameObject obj = ((InGameManager)SceneController).SpawnPlayer(true);
                    AgentController a = obj.GetComponent<AgentController>();
                    ((Client)Managers[Core.Managers.Client]).SendData((int)Events.InGame,(int)InGameTypes.EnterP,JsonUtility.ToJson(new TransformPaket(obj.transform.position,obj.transform.rotation)));
                    WinPoint = 0;
                    break;
                }
            case SceneTypes.Ending:
                {
                    ((EndingManager)SceneController).SetWin(winPoint >= 20);
                    break;
                }
        }
    }
    
}
