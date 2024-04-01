using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jc
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField] 
        private bool debug; // 디버그 모드 Gizmos
        [SerializeField]
        private float atk;
        [SerializeField] 
        private float range;
        [SerializeField, Range(0, 360)]
        private float angle;

        private float preAngle;
        private float cosAngle;
        private float CosAngle
        {
            get
            {
                if (preAngle == angle)
                    return cosAngle;

                preAngle = angle;
                cosAngle = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
                return cosAngle;
            }
        }

        // 범위내 적들을 모두 공격
        Collider[] colliders = new Collider[10];
        private void AttackTiming()
        {
            int size = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, Manager.Layer.monsterLM);
            for (int i = 0; i < size; i++)
            {
                Vector3 dirToTarget = (colliders[i].transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dirToTarget) < CosAngle)
                    continue;

                IDamageable damagable = colliders[i].GetComponent<IDamageable>();
                damagable?.TakeDamage(atk, transform.position);
            }
        }

        // 애니메이션 이벤트를 통한 공격처리
        public void Attack(float atk, Animator anim)
        {
            // 공격력 세팅
            this.atk = atk;
            anim.SetTrigger("OnAttack");
        }

        private void OnDrawGizmosSelected()
        {
            if (debug == false)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);

            Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
            Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

            Debug.DrawRay(transform.position, rightDir * range, Color.cyan);
            Debug.DrawRay(transform.position, leftDir * range, Color.cyan);
        }
    }
}
