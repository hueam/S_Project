using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T: PoolableMono
{
    private T prefab;
    private Stack<T> pool = new Stack<T>();
    private Transform parent;
    public Pool(T prefab,Transform parent, int count){
        this.prefab = prefab;
        this.parent = parent;
        for(int i = 0; i< count; i++)
        {
            T obj = GameObject.Instantiate(prefab,parent);
            obj.gameObject.name.Replace("(Clone)","");
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
    }
    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }
    public T Pop()
    {
        T obj;
        if(pool.Count > 0)
        {
            obj = pool.Pop();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(prefab,parent);
            obj.gameObject.name.Replace("(Clone)","");
        }
        return obj;
    }
}
