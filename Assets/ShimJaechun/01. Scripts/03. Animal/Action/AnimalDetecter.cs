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
            // 플레이어가 탐지범위에 들어온 경우
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            PlayerTrigger target = other.GetComponent<PlayerTrigger>();
            OnDetectTarget?.Invoke(target);
        }
        private void OnTriggerExit(Collider other)
        {
            // 플레이어가 탐지범위에서 벗어난 경우
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            OffDetectTarget?.Invoke();
        }
    }
}
