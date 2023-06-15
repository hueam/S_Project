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
    private Dictionary<string, OtherPlayer> players = new Dictionary<string,OtherPlayer>();
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
                    if(players.ContainsKey(data.GetValue().ToString()))
                    {
                        OtherPlayer otherPlayer = ((InGameManager)GameManager.Instance.SceneController).SpawnPlayer(false).GetComponent<OtherPlayer>();
                        otherPlayer.socketID = data.GetValue().ToString();
                        players[data.GetValue().ToString()] = otherPlayer;
                    }
                    else
                    {
                        OtherPlayer otherPlayer = ((RoomManager)GameManager.Instance.SceneController).SpawnPlayer(false).GetComponent<OtherPlayer>();
                        otherPlayer.socketID = data.GetValue().ToString();
                        players.Add(data.GetValue().ToString(),otherPlayer);
                    }
                 });
            });
            socket.On("otherDamage",(data)=>
            {   
                buffer.Enqueue(()=>{
                DamagePacket packet = JsonUtility.FromJson<DamagePacket>(data.GetValue().ToString());
                if(players.ContainsKey(packet.id))
                    players[packet.id].PlayerHealthCompo.HitDamage(packet.damage);
                else Define.Player.PlayerHealthCompo.HitDamage(packet.damage);
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
                TransformPaket paket = JsonUtility.FromJson<TransformPaket>(data.GetValue().ToString());
                if(players.ContainsKey(paket.id)){
                    buffer.Enqueue(() =>
                    {
                        if (players.ContainsKey(paket.id) && (players[paket.id] != null))
                            players[paket.id].SetVelocity(new Vector3(paket.x, paket.y, paket.z), new Quaternion(paket.rx, paket.ry, paket.rz, paket.rw));
                    });
                }
            });
            socket.On("otherReady",data=>
            {
                buffer.Enqueue(()=>((RoomManager)GameManager.Instance.SceneController).OtherReady(bool.Parse(data.GetValue().ToString())));
            });
            socket.On("message",(data)=>
            {
                Debug.Log(data.GetValue().ToString());
                buffer.Enqueue(()=>((UIManager)GameManager.Instance.Managers[Managers.UIManager]).ShowMessage(data.GetValue().ToString()));
            });
            socket.ConnectAsync();
            StartCoroutine(CheckServer());
    }
    public IEnumerator CheckServer()
    {
        yield return new WaitForSeconds(0.5f);
        try
        {
            if(IsConnect == false)
            {
                ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).ShowMessage("서버 접속 실패!");

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
            yield return new WaitForSeconds(1f/10f);
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
    InGame = 2,
}

public enum RoomTypes
{
    Ready = 0,
    Start = 1,
    ChangeScene = 2,
    Move = 3,
    InitP = 4,
}

public enum IntroTypes
{
    CreateRoom = 0,
    EnterRoom = 1,
    ChangeScene = 2,
}

public enum InGameTypes
{
    EnterP = 0,
    Hit = 1,
    Die = 2,
    ReSapwn = 3,
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