using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class MonsterDetecter : MonoBehaviour
    {
        // 공격을 가할 수 있는 오브젝트를 탐지
        // 매개변수 : 객체, 레이어 
        public UnityAction<GameObject> OnTrigger;
        public UnityAction<GameObject> OffTrigger;

        private void OnTriggerEnter(Collider other)
        {
            // 데미지를 입을 수 있는 즉, 공격이 가능한 객체일 경우 액션 
            if (other.GetComponent<IDamageable>() != null)
                OnTrigger?.Invoke(other.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IDamageable>() != null)
                OffTrigger?.Invoke(other.gameObject);
        }
    }
}
