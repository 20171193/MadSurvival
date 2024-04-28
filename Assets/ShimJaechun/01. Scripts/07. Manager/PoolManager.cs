using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{  
    // Key (InstanceID : 각 인스턴스 별 고유한 int값), Value (오브젝트 풀러)
    private Dictionary<int, ObjectPooler> poolDic = new Dictionary<int, ObjectPooler>();

    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        // 풀 생성 (각 오브젝트들이 위치하는 부모 오브젝트)
        GameObject gameObject = new GameObject();
        gameObject.name = $"Pool_{prefab.name}";

        ObjectPooler objectPool = gameObject.AddComponent<ObjectPooler>();
        objectPool.CreatePool(prefab, size, capacity);

        poolDic.Add(prefab.GetInstanceID(), objectPool);
    }

    public void DestroyPool(PooledObject prefab)
    {
        ObjectPooler objectPool = poolDic[prefab.GetInstanceID()];
        Destroy(objectPool.gameObject);

        poolDic.Remove(prefab.GetInstanceID());
    }

    public void ClearPool()
    {
        // 풀 비우기
        foreach (ObjectPooler objectPool in poolDic.Values)
        {
            Destroy(objectPool.gameObject);
        }

        poolDic.Clear();
    }

    public PooledObject GetPool(PooledObject prefab, Vector3 position, Quaternion rotation)
    {
        // 프리팹의 InstanceID로 로딩
        return poolDic[prefab.GetInstanceID()].GetPool(position, rotation);
    }
}