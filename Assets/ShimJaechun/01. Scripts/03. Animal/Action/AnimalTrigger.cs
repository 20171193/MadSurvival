using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class AnimalTrigger : MonoBehaviour, IDamageable, IKnockbackable, ITileable
    {
        [SerializeField]
        private Animal owner;

        public UnityAction OnTakeDamage;

        private Coroutine knockbackRoutine;

        public void TakeDamage(float value)
        {
            // 데미지값 처리
            float damage = value - owner.Stat.AMR;
            if (damage <= 0) return;

            // 사망처리
            if (owner.Stat.OwnHp < damage)
            {
                owner.FSM.ChangeState("Die");
            }
            // 데미지 처리
            else
            {
                owner.Anim.SetTrigger("OnHit");
                owner.Stat.OwnHp -= damage;
            }
        }
        public void Knockback(float power, float time, Vector3 suspectPos)
        {
            knockbackRoutine = StartCoroutine(KnockBackRoutine(power, time, suspectPos));
        }
        IEnumerator KnockBackRoutine(float power, float time, Vector3 suspectPos)
        {
            // 네비메시 비활성화
            // 물리 이동으로 전환

            Vector3 dir = transform.position - suspectPos;
            owner.transform.forward = -dir;
            owner.Agent.enabled = false;
            owner.Rigid.AddForce(dir * power, ForceMode.Impulse);

            yield return new WaitForSeconds(time);

            // 원복
            owner.Rigid.velocity = Vector3.zero;
            owner.Agent.enabled = true;

            // 넉백 후 딜레이
            owner.Agent.isStopped = true;
            yield return new WaitForSeconds(0.2f);
            owner.Agent.isStopped = false;

            // 회피형 : 회피상태 진입
            // 공격형 : 공격상태 진입
            OnTakeDamage?.Invoke();
        }

        public void OnTile(Ground ground)
        {
            owner.onGround = ground;
        }
    }
}