using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterDetecter : MonoBehaviour
{
    // 공격을 가할 수 있는 오브젝트를 탐지
    public UnityAction<IDamageable> OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if(target != null)
            OnTrigger?.Invoke(target);
    }
}
