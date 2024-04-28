using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System;
using UnityEngine.Events;
using Core;
using System.Net;

public class Client : MonoBehaviour, IManager
{
    public SocketIO socket = null;

    [Header("유니티 이벤트")]
    public UnityEvent ErrorEvent = null;

    [SerializeField]int port;
    [SerializeField]private bool IsConnect;

    [SerializeField]GameObject playerPrefab;
    private Queue<Action> buffer = new Queue<Action>();
    private Dictionary<string, OtherPlayer> players = new Dictionary<string,OtherPlayer>();
    public event Action alwayEvnet = null;
    public async void JoinServer()
    {
        if(socket != null) return;
        
        try
        {
            socket = new SocketIO($"http://{IPAddress.Parse(IPInput.IP).ToString()}:{port}");

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
                        OtherPlayer otherPlayer;
                        if(GameManager.Instance.SceneEnum == SceneTypes.Room)
                           otherPlayer =((RoomManager)GameManager.Instance.SceneController).SpawnPlayer(false).GetComponent<OtherPlayer>();
                        else
                           otherPlayer = ((InGameManager)GameManager.Instance.SceneController).SpawnPlayer(false).GetComponent<OtherPlayer>();
                        otherPlayer.socketID = data.GetValue().ToString();
                        players[data.GetValue().ToString()] = otherPlayer;
                    }
                    else
                    {
                        OtherPlayer otherPlayer;
                        if(GameManager.Instance.SceneEnum == SceneTypes.Room)
                           otherPlayer =((RoomManager)GameManager.Instance.SceneController).SpawnPlayer(false).GetComponent<OtherPlayer>();
                        else
                           otherPlayer = ((InGameManager)GameManager.Instance.SceneController).SpawnPlayer(false).GetComponent<OtherPlayer>();
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
                    players[packet.id].currentHP -= packet.damage;
                else Define.Player.PlayerHealthCompo.HitDamage(packet.damage);
                });
            });
            socket.On("otherDie",(data)=>
            {
                string id = data.GetValue().ToString();
                buffer.Enqueue(() =>
                {
                    if (players.ContainsKey(id))
                        players[id].SetDie();
                });
            });
            socket.On("exitOther",(data)=>
            {
                buffer.Enqueue(()=>{
                    if((int)GameManager.Instance.SceneEnum > 0)
                    {
                        Destroy(players[data.GetValue().ToString()].gameObject);
                        players.Remove(data.GetValue().ToString());
                    }
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
            socket.On("Fire",data => {
                Vec3Packet p = JsonUtility.FromJson<Vec3Packet>(data.GetValue().ToString());
                buffer.Enqueue(()=>{
                    players[p.id].Fire(new Vector3(p.x,p.y,p.z));
                });
            });
            socket.On("MoveOther",(data)=>
            {
                TransformPaket paket = JsonUtility.FromJson<TransformPaket>(data.GetValue().ToString());
                Debug.Log("움직임");
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
            socket.On("otherReSapwn",data=>
            {
                TransformPaket p = JsonUtility.FromJson<TransformPaket>(data.GetValue().ToString());
                buffer.Enqueue(()=>players[p.id].ReSapwn(new Vector3(p.x,p.y,p.y),new Quaternion(p.rx,p.ry,p.rz,p.rw)));
            });
            socket.On("message",(data)=>
            {
                Debug.Log(data.GetValue().ToString());
                buffer.Enqueue(()=>((UIManager)GameManager.Instance.Managers[Managers.UIManager]).ShowMessage(data.GetValue().ToString()));
            });
            await socket.ConnectAsync();

            if (IsConnect == false)
            {
                ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).ShowMessage("서버 접속 실패!");

                throw new Exception("접속하지못하였음");
            }
        }
        catch (Exception err)
        {
            Debug.LogException(err);
            ErrorEvent?.Invoke();
            socket = null;
        }

        ((IntroManager)GameManager.Instance.SceneController).NextGate();
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
        GameManager.Instance.roomID = roomID;
        SendData((int)Events.Intro,(int)IntroTypes.CreateRoom,roomID);
    }
    public void JoinRoom(string roomID)
    {
        GameManager.Instance.roomID = roomID;
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
    public void envetClear()
    {
        alwayEvnet = null;
    }
    public void ExitGame()
    {
        socket.EmitAsync("disconnect");
    }

    public void Init(Transform parent)
    {
    }
    private void OnApplicationQuit() {
        socket?.EmitAsync("disconnect");
    }
}


public enum Events
{
    Intro = 0,
    Room = 1,
    InGame = 2,
    Ending = 3,
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
    Ending = 4,
    Fire = 5,
}
public enum SceneTypes
{
    Intro = 0,
    Room = 1,
    InGame = 2,
    Ending = 3,
}
public enum EndingTypes
{
    Goto = 0,
}