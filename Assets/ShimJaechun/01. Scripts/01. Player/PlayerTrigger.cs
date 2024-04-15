using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using jungmin;
namespace Jc
{
    // 충돌관련 이벤트처리
    public class PlayerTrigger : MonoBehaviour, IDamageable
    {
        public Player owner;

        public void TakeDamage(float value)
        {
            float damage = value - owner.Stat.AMR;
            if (damage < 1) return;


            owner.Stat.OwnHp -= damage;
            if (owner.Stat.OwnHp > 0)
            {
                owner.Anim.SetTrigger("OnHit");
                owner.OnTakeDamage();
            }
        }
        public void GetItem(Item item)
        {
            owner.GetItem(item);
        }


        private void OnTriggerStay(Collider other)
        {
            // 물에 근접하고 있는 경우
            if(Manager.Layer.waterLM.Contain(other.gameObject.layer))
            {
                owner.IsOnWater = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // 물에서 벗어난 경우
            if (Manager.Layer.waterLM.Contain(other.gameObject.layer))
            {
                owner.IsOnWater = false;
            }
        }
    }
}
