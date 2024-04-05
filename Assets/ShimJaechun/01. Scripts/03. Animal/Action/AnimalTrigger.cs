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
            // �������� ó��
            float damage = value - owner.Stat.AMR;
            if (damage <= 0) return;

            // ���ó��
            if (owner.Stat.OwnHp < damage)
            {
                owner.FSM.ChangeState("Die");
            }
            // ������ ó��, �˹�
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
        //    // �׺�޽� ��Ȱ��ȭ
        //    // ���� �̵����� ��ȯ
        //    agent.enabled = false;
        //    rigid.AddForce(transform.forward * -knockBackPower, ForceMode.Impulse);

        //    yield return new WaitForSeconds(KnockBackTime);

        //    // ����
        //    rigid.velocity = Vector3.zero;
        //    agent.enabled = true;

        //    // �˹� �� ������
        //    agent.isStopped = true;
        //    yield return new WaitForSeconds(0.2f);
        //    agent.isStopped = false;
        //}
    }
}