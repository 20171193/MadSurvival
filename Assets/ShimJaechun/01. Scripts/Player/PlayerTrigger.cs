using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    // 충돌관련 이벤트처리
    public class PlayerTrigger : MonoBehaviour, ITileable, IDamageable
    {
        public Player owner;

        public void OnTile(Ground ground)
        {
            owner.currentGround = ground;
        }

        public void TakeDamage(float damage, Vector3 suspectPos)
        {
            owner.CurHp -= damage;
            if(owner.CurHp <= 0)
                owner.OnDie();
            else
                owner.OnTakeDamage();
        }
    }
}
