using System.Buffers;
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

        [Header("������ ���� - AI����")]
        [SerializeField]
        private float trackingTime;
        public float TrackingTime { get { return trackingTime; } }

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
        }

        private void OnEnable()
        {
            Manager.Navi.OnChangePlayerGround += OnPlayerGround;
        }
        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnPlayerGround;
        }

        protected override void Start()
        {
            base.Start();

            attacker.Range = stat.AtkRange;
            attackTrigger.AtkCol.radius = stat.AtkRange - 0.1f;
        }

        public void OnTracking()
        {
            if (fsm.FSM.CurState == "Die" || fsm.FSM.CurState == "ReturnPool") return;
            fsm.FSM.ChangeState("Tracking");
        }

        public void OnPlayerGround(Ground ground)
        {
            playerGround = ground;
        }

        public override void OnDetectTarget(PlayerTrigger player)
        {
            base.OnDetectTarget(player);
            isLose = false;

            // ������ ������ ��� Ž������ �� �ٷ� �߰�
            if (!isNeutral)
                OnTracking();
        }
        public override void OnLoseTarget()
        {
            isLose = true;
        }
    }
}
