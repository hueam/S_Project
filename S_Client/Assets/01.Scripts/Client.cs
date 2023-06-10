using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System;
using UnityEngine.Events;
using Core;

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
    public UnityEvent ErrorEvent = null;

    [SerializeField]string URL;
    [SerializeField]int port;
    [SerializeField]private bool IsConnect;

    [SerializeField]GameObject playerPrefab;
    private Queue<Action> buffer = new Queue<Action>();
    private Dictionary<string, GameObject> players = new Dictionary<string,GameObject>();
    public event Action alwayEvnet = null;
    public void JoinServer()
    {
        if(socket != null) return;
        
            socket = new SocketIO($"{URL}:{port}");
            socket.OnError += (sender, e) =>
            {
                Debug.LogError("Socket.IO error: " + e);
            };
            socket.On("connection", (data)=>{
                buffer.Enqueue(()=>{
                    IsConnect = bool.Parse(data.GetValue().ToString());
                    Debug.Log(IsConnect);
                }
                );
            });
            socket.On("enterOther",(data)=>
            {
               buffer.Enqueue(()=>{
                    Debug.Log(data.GetValue().ToString());
                    players.Add(data.GetValue().ToString(),((RoomManager)GameManager.Instance.SceneController).SpawnPlayer(false));
                });
            });
            socket.On("error", (data)=>
            {
                Debug.Log(data);
            });
            socket.On("ChangeScene", (data)=>
            {
                int scnenNum = int.Parse(data.GetValue().ToString());
                buffer.Enqueue(() => 
                {
                    GameManager.Instance.LoadScene(scnenNum);
                    
                });
            });
            socket.On("MoveOther",(data)=>
            {
                buffer.Enqueue(()=>{
                    TransformPaket paket = JsonUtility.FromJson<TransformPaket>(data.GetValue().ToString());
                    players[paket.id].transform.position = new Vector3(paket.x,paket.y,paket.z);
                    players[paket.id].transform.rotation = new Quaternion(paket.rx,paket.ry,paket.rz,paket.rw);
                });
            });
            socket.On("otherReady",data=>
            {
                buffer.Enqueue(()=>((RoomManager)GameManager.Instance.SceneController).OtherReady(bool.Parse(data.GetValue().ToString())));
            });
            socket.On("Play",(data)=>
            {
                ((RoomManager)GameManager.Instance.SceneController).GamePlay();
            });
            socket.On("message",(data)=>
            {
                Debug.Log(data.GetValue().ToString());
                buffer.Enqueue(()=>((UIManager)GameManager.Instance.Managers[Managers.UIManager]).ShowMessage(data.GetValue().ToString()));
            });
            socket.ConnectAsync();
            StartCoroutine(CheckServer());
            Debug.Log(Time.time);
    }
    public IEnumerator CheckServer()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(12);
        try
        {
            if(IsConnect == false)
            {
                ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).SendMessage("서버 접속 실패!");
                throw new Exception("접속하지못하였음");
            }
        }
        catch(Exception err)
        {
            Debug.LogException(err);
            ErrorEvent?.Invoke();
            socket = null;
        }
        StartCoroutine(SendMessage());
    }
    private IEnumerator SendMessage()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f/60f);
            alwayEvnet?.Invoke();
        }
    }
    void Update()
    {
        if (socket == null)
            return;
        if(buffer.Count > 0&& GameManager.Instance.IsLoad == false)
        {
            buffer.Dequeue()?.Invoke();
        }
    }
    public void CreateRoom(string roomID)
    {
        SendData((int)Events.Intro,(int)IntroTypes.CreateRoom,roomID);
    }
    public void JoinRoom(string roomID)
    {
        SendData((int)Events.Intro,(int)IntroTypes.EnterRoom,roomID);
    }
    public void SendData(int events,int type, string value)
    {
        Packet p =new Packet(events,type,value);
        // Debug.Log(p);
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
    Intro = 0,
    Room = 1,
    Ending,
}

public enum RoomTypes
{
    Ready = 0,
    Start = 1,
    ChangeScene = 2,
    Move = 3,
}

public enum IntroTypes
{
    CreateRoom = 0,
    EnterRoom = 1,
    ChangeScene = 2,
}

public enum EndingTypes
{
    
}
public enum SceneTypes
{
    Intro = 0,
    Room = 1,
    InGame = 2,

}

public enum AlwayEventType
{
    Transform = 0,

}