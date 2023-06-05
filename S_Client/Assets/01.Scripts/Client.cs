using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System;
using UnityEngine.Events;
using Define;

public class Client : MonoBehaviour, IManager
{
    private static Client instance;
    public static Client Instance {
        get{
            if(instance == null)
            {
                instance = FindObjectOfType<Client>();
            }
            return instance;
        }
    }

    private SocketIO socket = null;

    [Header("유니티 이벤트")]
    public UnityEvent<Exception> ErrorEvent = null;
    [SerializeField]string URL;
    [SerializeField]int port;
    //List<Player> players = new List<Player>();

    private Queue<Action> buffer = new Queue<Action>();
    private Dictionary<string, GameObject> players = new Dictionary<string,GameObject>();
    public void JoinServer()
    {
        if(socket != null) return;
        try{
            socket = new SocketIO($"{URL}:{port}");
            socket.On("connection", (data)=>{
                Debug.Log(data);
            });
            socket.On("error", (data)=>
            {
                Debug.Log(data);
            });
            socket.On("message",(data)=>
            {
                Debug.Log(data.GetValue().ToString());
                buffer.Enqueue(()=>((UIManager)GameManager.Instance.Managers[Define.Managers.UIManager]).ShowMessage(data.GetValue().ToString()));
            });
    
            socket.ConnectAsync();
        }
        catch(Exception err)
        {
            Debug.LogException(err);
            ErrorEvent?.Invoke(err);
        }
    }
    void Update()
    {
        if (socket == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendData((int)Events.Intro,(int)IntroTypes.EnterRoom,"fucking socketio");
        }
        if(buffer.Count > 0)
        {
            buffer.Dequeue()?.Invoke();
        }
    }
    public void CreateServer()
    {
        SendData((int)Events.Intro,(int)IntroTypes.CreateRoom,"fucking socketio");
    }
    public void SendData(int events,int type, string value)
    {
        Packet p =new Packet(events,type,value);
        Debug.Log(p);
        socket?.EmitAsync("message",JsonUtility.ToJson(p));
    } 
    public void SendData(Packet data)
    {
        socket?.EmitAsync("message",JsonUtility.ToJson(data));
    }

    public void Init(Transform parent)
    {
    }
}


public enum Events
{
    Intro,
    InGame =0,
    Ending,
}

public enum InGameTypes
{
    Move =0,
}

public enum IntroTypes
{
    CreateRoom = 0,
    EnterRoom = 1,
}

public enum EndingTypes
{
    
}