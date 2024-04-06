using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class AttackAnimal : Animal
    {
        [Header("Linked Class")]
        [SerializeField]
        private AnimalAttacker attacker;
        public AnimalAttacker Attacker { get { return attacker; } }

        [SerializeField]
        private AnimalTracker tracker;
        public AnimalTracker Tracker { get { return tracker; } }

        [SerializeField]
        private AnimalAttackTrigger attackTrigger;
        public AnimalAttackTrigger AttackTrigger { get { return attackTrigger; } }

        [Header("에디터 세팅 - AI스텟")]
        [SerializeField]
        private float trackingTime;
        public float TrackingTime { get { return trackingTime; } }

        [SerializeField]
        private float attackRange;
        public float AttackRange { get { return attackRange; } }

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private bool isLose = false;
        public bool IsLose { get { return isLose; } set { isLose = value; } }

        protected override void Awake()
        {
            base.Awake();

            fsm.CreateFSM(this);
            fsm.FSM.AddState("Tracking", new AnimalTracking(this));
            fsm.FSM.AddState("Attack", new AnimalAttack(this));
            fsm.FSM.Init("Pooled");

            attacker.Range = attackRange;
            Manager.Navi.OnChangePlayerGround += OnPlayerGround;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public void OnTracking()
        {

        }

        public void OnPlayerGround(Ground ground)
        {
            playerGround = ground;
        }

        public override void OnDetectTarget(PlayerTrigger player)
        {
            base.OnDetectTarget(player);
            isLose = false;

            // 공격형 동물의 경우 탐지했을 때 바로 추격
            if (!isNeutral)
                OnTracking();
        }
        public override void OnLoseTarget()
        {
            isLose = true;
        }
    }
}
