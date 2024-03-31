using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    // Ǯ���� ������Ʈ�� ���̽� Ŭ����
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
