using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class AnimalAttackTrigger : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private AttackAnimal owner;

        [SerializeField]
        private SphereCollider col;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private GameObject currentTarget;
        public GameObject CurrentTarget { get { return currentTarget; } }

        public UnityAction<GameObject> OnAttackTrigger;

        private void Awake()
        {
            col.radius = owner.AttackRange - 0.1f;
        }

        private void OnTriggerEnter(Collider other)
        {
            // �������� ���� �� �ִ� ��, ������ ������ ��ü�� ��� �׼� 
            if (other.GetComponent<IDamageable>() != null)
            {
                if (owner.FSM.FSM.CurState == "Tracking" &&
                (owner.Tracker.IsTrackingPlayer && other.gameObject.tag == "Player" ||
                !owner.Tracker.IsTrackingPlayer && Manager.Layer.wallLM.Contain(other.gameObject.layer)))
                {
                    // Ÿ������ ����
                    currentTarget = other.gameObject;

                    owner.FSM.ChangeState("Attack");
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                // ������ Ÿ�� ����
                if (other.gameObject == currentTarget)
                    currentTarget = null;
            }
        }
    }
}
