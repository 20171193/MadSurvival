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
        private AnimalAttackTrigger attackTrigger;
        public AnimalAttackTrigger AttackTrigger { get { return attackTrigger; } }

        [Header("������ ���� - AI����")]
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
            attacker.Range = attackRange;
        }

        public override void OnDetectTarget(PlayerTrigger player)
        {
            base.OnDetectTarget(player);
            isLose = false;
        }
        public override void OnLoseTarget()
        {
            isLose = true;
        }
    }
}
