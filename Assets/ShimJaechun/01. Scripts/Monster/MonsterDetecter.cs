using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterDetecter : MonoBehaviour
{
    // ������ ���� �� �ִ� ������Ʈ�� Ž��
    public UnityAction<IDamageable> OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if(target != null)
            OnTrigger?.Invoke(target);
    }
}
