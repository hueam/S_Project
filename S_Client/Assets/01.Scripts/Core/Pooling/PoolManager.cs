using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour,IManager
{
    [SerializeField]private PoolListSO poolList = null;
    private Dictionary<string,Pool<PoolableMono>> pools = new Dictionary<string, Pool<PoolableMono>>();


    public void Init(Transform parent)
    {
        poolList.PoolList.ForEach(p => {
            pools.Add(p.Prefab.name,new Pool<PoolableMono>(p.Prefab,transform,p.Count));
        });
    }
    public PoolableMono Pop(string name)
    {
        if(pools.ContainsKey(name))
        {
            PoolableMono obj = pools[name].Pop();
            obj.Init();
            return obj;
        }
        Debug.LogError("없는 이름의 오브젝트입니다");
        return null;
    }
    public void Push(PoolableMono obj)
    {
        pools[obj.name].Push(obj);
    }
}
