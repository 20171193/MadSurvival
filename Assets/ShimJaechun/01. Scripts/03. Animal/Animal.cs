using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Jc
{
    public class Animal : MonoBehaviour
    {
        [Header("������ ����")]
        [Space(2)]
        [SerializeField]
        protected string animalName;
        public string AnimalName { get { return animalName; } }

        [SerializeField]
        protected NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        [SerializeField]
        protected Animator anim;
        public Animator Anim { get { return anim; } }

        // �߸��� ��������?
        [SerializeField]
        protected bool isNeutral;
        public bool IsNeutral { get { return isNeutral; } }

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        protected AnimalFSM fsm;
        public AnimalFSM FSM { get { return fsm; } }

        [SerializeField]
        protected AnimalStat stat;
        public AnimalStat Stat { get { return stat;} }

        [SerializeField]
        protected AnimalDetecter detecter;
        public AnimalDetecter Detecter { get { return detecter; } }

        [SerializeField]
        protected AnimalTrigger trigger;
        public AnimalTrigger Trigger { get { return trigger; } }

        protected virtual void Awake()
        {
            detecter.GetComponent<SphereCollider>().radius = stat.DetectRange;
        }

        protected virtual void OnEnable()
        {
            // Ȱ��ȭ �� Ǯ������ -> �����·� ��ȯ
            fsm?.ChangeState("Idle");
        }

        // Ž������ �����
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stat.DetectRange);
        }
    }
}
