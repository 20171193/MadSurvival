using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class MonsterDetecter : MonoBehaviour
    {
        // ������ ���� �� �ִ� ������Ʈ�� Ž��
        // �Ű����� : ��ü, ���̾� 
        public UnityAction<GameObject> OnTrigger;
        public UnityAction<GameObject> OffTrigger;

        private void OnTriggerEnter(Collider other)
        {
            // �������� ���� �� �ִ� ��, ������ ������ ��ü�� ��� �׼� 
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
