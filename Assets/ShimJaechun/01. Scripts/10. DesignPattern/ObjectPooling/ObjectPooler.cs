using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Pool;

// 각 오브젝트를 풀링할 때 생성될 오브젝트 풀러
public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    protected Stack<PooledObject> objectPool;   // 풀 Stack

    [SerializeField]    
    protected PooledObject prefab;  // 풀링할 오브젝트

    [SerializeField]
    protected int size;     // 할당한 크기

    [SerializeField]
    protected int capacity; // 용량

    public virtual void CreatePool(PooledObject prefab, int size, int capacity)
    {
        this.prefab = prefab;
        this.size = size;
        this.capacity = capacity;

        objectPool = new Stack<PooledObject>(capacity);

        for (int i = 0; i < size; i++)
        {
            PooledObject instance = Instantiate(prefab);
            instance.gameObject.SetActive(false);
            instance.Pooler = this;
            instance.transform.parent = transform;
            objectPool.Push(instance);
        }
    }
    public virtual PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        if (objectPool.Count > 0)
        {
            PooledObject instance = objectPool.Pop();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);
            return instance;
        }
        // 풀에 남은 오브젝트가 없을경우 생성
        else
        {
            PooledObject instance = Instantiate(prefab);
            instance.Pooler = this;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }
    }
    public void ReturnPool(PooledObject instance)
    {
        if (instance == null) return;

        if (objectPool.Count < capacity)
        {
            instance.gameObject.SetActive(false);
            instance.transform.parent = transform;
            objectPool.Push(instance);
        }
        // 풀의 용량이 모두 찬경우 해당 오브젝트를 제거
        else
            Destroy(instance.gameObject);
    }
}