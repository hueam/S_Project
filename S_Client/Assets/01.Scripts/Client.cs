using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System;

public class Client : MonoBehaviour
{
    public static Client Instance {
        get{
            if(instance == null)
            {
                instance = FindObjectOfType<Client>();
            }
            return instance;
        }
    }
    private static Client instance;

    public event Action subscribeEvents = null;
    private SocketIO socket;
    [SerializeField]string URL;
    [SerializeField]int port;
    //List<Player> players = new List<Player>();
    [SerializeField] GameObject playerObj;

    private Queue<Action> buffer = new Queue<Action>();
    private Dictionary<string, GameObject> players = new Dictionary<string,GameObject>();

    void Start()
    {
        socket = new SocketIO($"{URL}:{port}");
        socket.On("connection", (data)=>{
            Debug.Log(data);
        });
        socket.On("error", (data)=>
        {
            Debug.Log(data);
        });
        socket.On("exit", (data)=>
        {
            Debug.Log("플레이어 한명이 나가셨습니다");
        });
        socket.On("anthorEnter",(data)=>
        {
            buffer.Enqueue(()=>{
                GameObject obj = Instantiate(playerObj);
                players.Add(data.GetValue().ToString(),obj);
            });
        });
        socket.On("move", (data)=>{
            buffer.Enqueue(()=>{
                TransformPaket t = JsonUtility.FromJson<TransformPaket>(data.GetValue().ToString());
                players[t.id].transform.position = new Vector3(t.x,t.y,t.z);
                players[t.id].transform.rotation = new Quaternion(t.rx,t.ry,t.rz,t.rw);
            });
        });

        socket.ConnectAsync();
        StartCoroutine(SendSubscribeEvents());
    }
    private IEnumerator SendSubscribeEvents()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f/20f);
            subscribeEvents?.Invoke();
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
            SendData((int)Events.Intro,(int)IntroTypes.CreateRoom,"fucking socketio");
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
    public void SendData(int events,int type, string value)
    {
        Packet p =new Packet(events,type,JsonUtility.ToJson(new RoomPacket(value,10)));
        Debug.Log(p);
        socket?.EmitAsync("message",JsonUtility.ToJson(p));
    } 
    public void SendData(Packet data)
    {
        socket?.EmitAsync("message",JsonUtility.ToJson(data));
    }
}


public enum Events
{
    InGame =0,
    Intro,
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