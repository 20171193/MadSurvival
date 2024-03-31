using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    // 풀링될 오브젝트의 베이스 클래스
    private ObjectPooler pooler;
    public ObjectPooler Pooler { get { return pooler; } set { pooler = value; } }

    public virtual void Release()
    {
        if (pooler != null)
            pooler.ReturnPool(this);
        else
            Destroy(gameObject);
    }
}
