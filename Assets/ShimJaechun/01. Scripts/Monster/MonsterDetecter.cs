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

        private void OnTriggerEnter(Collider other)
        {
            // �������� ���� �� �ִ� ��, ������ ������ ��ü�� ��� �׼� 
            if (other.GetComponent<IDamageable>() != null)
                OnTrigger?.Invoke(other.gameObject);
        }
    }
}
