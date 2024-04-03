using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class ExplosionInvoker : PooledObject
    {
        [Header("����� �ݰ�")]
        public float radius;
        [Header("������� ��")]
        public float power;
        [Header("���� ������ �ð�")]
        public float releaseTime;

        private Coroutine releaseRoutine;
        public void OnExplosion()
        {
            releaseRoutine = StartCoroutine(Extension.DelayRoutine(releaseTime, () => Release()));
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius, Manager.Layer.dropItemLM);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.transform.parent.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.useGravity = true;
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
                }
            }
        }
    }
}
