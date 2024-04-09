using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using jungmin;
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

        public void TakeDamage(float damage)
        {
            owner.Stat.OwnHp -= damage;
            if(owner.Stat.OwnHp <= 0)
                owner.OnDie();
            else
                owner.OnTakeDamage();
        }

        public void GetItem(Item item)
        {
            owner.GetItem(item);
        }
    }
}
