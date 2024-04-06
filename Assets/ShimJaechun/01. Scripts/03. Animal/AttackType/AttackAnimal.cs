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
        private PlayerTrigger curTarget;
        public PlayerTrigger CurTarget { get { return curTarget; } }

        [SerializeField]
        private bool isLose = false;
        public bool IsLose { get { return isLose; } set { isLose = value; } }
        
        protected override void Awake()
        {
            base.Awake();
            attacker.Range = attackRange;
        }

        public void DetectTarget(PlayerTrigger target)
        {
            isLose = false;
            curTarget = target;
        }
        public void LoseTarget()
        {
            isLose = true;
        }
    }
}
