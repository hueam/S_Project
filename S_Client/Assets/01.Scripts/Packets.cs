using UnityEngine;

[System.Serializable]
public struct TransformPaket
{
    public TransformPaket(Vector3 pos, Quaternion rot){
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.rx = rot.x;
        this.rz = rot.z;
        this.ry = rot.y;
        this.rw = rot.w;
        this.id = "";
    }
    public float x;
    public float y;
    public float z;
    public float rx;
    public float ry;
    public float rz;
    public float rw;
    public string id;
    public override string ToString()
    {
        return $"{x},{y},{z}";
    }
}


[System.Serializable]
public struct RoomPacket
{
    public RoomPacket(string roomID,int size)
    {
        this.roomID = roomID;
        this.size = size;
    }
    public string roomID;
    public int size;
}

[System.Serializable]
public struct Packet
{
    public Packet(int eventType,int type, string value){
        e = eventType;
        t = type;
        v = value; 
    }
    public int e;
    public int t;
    public string v;
    public override string ToString()
    {
        return $"{e},{t},{v}";
    }
}
public struct Vec3Packet
{
    public Vec3Packet(Vector3 pos){
        x = pos.x;
        y = pos.y;
        z = pos.z; 
        id ="";
    }
    public float x;
    public float y;
    public float z;
    public string id;
}

[System.Serializable]
public struct AttackPacket
{
    public int t;
    public int d;
    public string target;
}
public struct InitcharPacket{
    public Vector3 v;
    public Quaternion q;
}
public struct DamagePacket
{
    public DamagePacket(string id, int damage)
    {
        this.id = id;
        this.damage = damage;
    }
    public string id;
    public int damage;
}