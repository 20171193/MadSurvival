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
            // 플레이어가 탐지범위에 들어온 경우
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            currentTarget = other.GetComponent<PlayerTrigger>();
            OnDetectTarget?.Invoke(other.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            // 플레이어가 탐지범위에서 벗어난 경우
            if (!Manager.Layer.playerableLM.Contain(other.gameObject.layer)) return;

            OffDetectTarget?.Invoke();
            currentTarget = null;
        }
    }
}
