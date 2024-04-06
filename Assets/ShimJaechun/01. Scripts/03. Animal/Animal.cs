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
        protected Rigidbody rigid;
        public Rigidbody Rigid { get { return rigid; } }

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

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]

        [SerializeField]
        public Ground onGround;

        [SerializeField]
        public Ground playerGround;

        [SerializeField]
        protected PlayerTrigger curTarget;
        public PlayerTrigger CurTarget { get { return curTarget; } }


        protected virtual void Awake()
        {
            // Ž������ ����
            detecter.GetComponent<SphereCollider>().radius = stat.DetectRange;
            detecter.OnDetectTarget += OnDetectTarget;
            detecter.OffDetectTarget += OnLoseTarget;
        }
        protected virtual void OnEnable()
        {
            // Ȱ��ȭ �� Ǯ������ -> �����·� ��ȯ
            fsm?.ChangeState("Idle");
        }
        public virtual void OnDetectTarget(PlayerTrigger player)
        {
            curTarget = player;
        }
        public virtual void OnLoseTarget()
        {
            curTarget = null;
        }

        // Ž������ �����
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stat.DetectRange);
        }
    }
}
