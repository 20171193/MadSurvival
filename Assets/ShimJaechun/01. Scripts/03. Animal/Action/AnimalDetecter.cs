using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class AnimalDetecter : MonoBehaviour
    {
        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private PlayerTrigger currentTarget;
        public PlayerTrigger CurrentTarget { get { return currentTarget; } }

        public UnityAction<GameObject> OnDetectTarget;
        public UnityAction OffDetectTarget;

        private void OnTriggerEnter(Collider other)
        {
            // �÷��̾ Ž�������� ���� ���
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            currentTarget = other.GetComponent<PlayerTrigger>();
            OnDetectTarget?.Invoke(other.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            // �÷��̾ Ž���������� ��� ���
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            OffDetectTarget?.Invoke();
            currentTarget = null;
        }
    }
}
