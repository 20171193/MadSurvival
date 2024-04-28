using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{  
    // Key (InstanceID : �� �ν��Ͻ� �� ������ int��), Value (������Ʈ Ǯ��)
    private Dictionary<int, ObjectPooler> poolDic = new Dictionary<int, ObjectPooler>();

    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        // Ǯ ���� (�� ������Ʈ���� ��ġ�ϴ� �θ� ������Ʈ)
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
        // Ǯ ����
        foreach (ObjectPooler objectPool in poolDic.Values)
        {
            Destroy(objectPool.gameObject);
        }

        poolDic.Clear();
    }

    public PooledObject GetPool(PooledObject prefab, Vector3 position, Quaternion rotation)
    {
        // �������� InstanceID�� �ε�
        return poolDic[prefab.GetInstanceID()].GetPool(position, rotation);
    }
}