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
            // �������� ó��
            float damage = value - owner.Stat.AMR;
            if (damage <= 0) return;

            // ���ó��
            if (owner.Stat.OwnHp < damage)
            {
                owner.FSM.ChangeState("Die");
            }
            // ������ ó��
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
            // �׺�޽� ��Ȱ��ȭ
            // ���� �̵����� ��ȯ

            Vector3 dir = transform.position - suspectPos;
            owner.transform.forward = -dir;
            owner.Agent.enabled = false;
            owner.Rigid.AddForce(dir * power, ForceMode.Impulse);

            yield return new WaitForSeconds(time);

            // ����
            owner.Rigid.velocity = Vector3.zero;
            owner.Agent.enabled = true;

            // �˹� �� ������
            owner.Agent.isStopped = true;
            yield return new WaitForSeconds(0.2f);
            owner.Agent.isStopped = false;

            // ȸ���� : ȸ�ǻ��� ����
            // ������ : ���ݻ��� ����
            OnTakeDamage?.Invoke();
        }

        public void OnTile(Ground ground)
        {
            owner.onGround = ground;
        }
    }
}