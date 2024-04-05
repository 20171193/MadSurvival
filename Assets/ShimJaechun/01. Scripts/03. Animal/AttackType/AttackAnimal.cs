using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class AttackAnimal : Animal
    {
        [Header("������ ���� - AI����")]
        [SerializeField]
        private float trackingTime;
        public float TrackingTime { get { return trackingTime; } }

        [SerializeField]
        protected AnimalAttacker attacker;
        public AnimalAttacker Attacker { get { return attacker; } }
    }
}
