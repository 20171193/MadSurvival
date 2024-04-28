using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    // Ǯ���� ������Ʈ�� ���̽� Ŭ����
    protected ObjectPooler pooler;
    public ObjectPooler Pooler { get { return pooler; } set { pooler = value; } }

    // Ǯ���� ����
    [SerializeField]
    protected int size;
    public int Size { get { return size; } }

    // ������Ʈ�� ��Ȱ��ȭ
    public virtual void Release()
    {
        if (pooler != null)
            pooler.ReturnPool(this);
        else
            Destroy(gameObject);
    }
}
