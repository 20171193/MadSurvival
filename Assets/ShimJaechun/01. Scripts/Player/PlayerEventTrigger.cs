using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Jc
{
    public class PlayerEventTrigger : MonoBehaviour
    {
        [SerializeField]
        private PlayerAttacker attacker;
        public void OnAttack()
        {
            attacker.AttackTiming();
        }
    }
}
