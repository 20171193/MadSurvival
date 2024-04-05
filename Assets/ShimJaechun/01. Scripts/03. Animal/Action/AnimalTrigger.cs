using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class AnimalTrigger : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Animal owner;

        public UnityAction OnTakeDamage;        

        public void TakeDamage(float value, Vector3 suspectPos)
        {
            // 데미지값 처리
            float damage = value - owner.Stat.AMR;
            if (damage <= 0) return;

            // 사망처리
            if (owner.Stat.OwnHp < damage)
            {
                owner.FSM.ChangeState("Die");
            }
            // 데미지 처리, 넉백
            else
            {
                OnTakeDamage?.Invoke();
                owner.Anim.SetTrigger("OnHit");
                owner.Stat.OwnHp -= damage;
               // knockBackTimer = StartCoroutine(KnockBackRoutine());
            }
        }
        //IEnumerator KnockBackRoutine()
        //{
        //    // 네비메시 비활성화
        //    // 물리 이동으로 전환
        //    agent.enabled = false;
        //    rigid.AddForce(transform.forward * -knockBackPower, ForceMode.Impulse);

        //    yield return new WaitForSeconds(KnockBackTime);

        //    // 원복
        //    rigid.velocity = Vector3.zero;
        //    agent.enabled = true;

        //    // 넉백 후 딜레이
        //    agent.isStopped = true;
        //    yield return new WaitForSeconds(0.2f);
        //    agent.isStopped = false;
        //}
    }
}