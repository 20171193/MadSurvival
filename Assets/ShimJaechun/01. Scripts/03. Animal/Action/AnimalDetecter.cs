using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

namespace Jc
{
    public class AnimalDetecter : MonoBehaviour
    {
        public UnityAction<PlayerTrigger> OnDetectTarget;
        public UnityAction OffDetectTarget;

        private void OnTriggerEnter(Collider other)
        {
            // �÷��̾ Ž�������� ���� ���
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            PlayerTrigger target = other.GetComponent<PlayerTrigger>();
            OnDetectTarget?.Invoke(target);
        }
        private void OnTriggerExit(Collider other)
        {
            // �÷��̾ Ž���������� ��� ���
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            OffDetectTarget?.Invoke();
        }
    }
}
