using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Jc
{
    public class Animal : PooledObject
    {
        [Header("에디터 세팅")]
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

        [SerializeField]
        private SphereCollider detectCol;
        public SphereCollider DetectCol { get { return detectCol; } }

        // 중립형 몬스터인지?
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
            detecter.OnDetectTarget += OnDetectTarget;
            detecter.OffDetectTarget += OnLoseTarget;
        }
        protected virtual void Start()
        {
            stat.InitSetting();
            detectCol.radius = stat.DetectRange;
        }
        public virtual void OnDetectTarget(PlayerTrigger player)
        {
            curTarget = player;
        }
        public virtual void OnLoseTarget()
        {
            curTarget = null;
        }

        // 탐지범위 디버깅
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stat.DetectRange);
        }
    }
}
