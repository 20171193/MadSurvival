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
        public UnityAction OnTakeDamage;
        public UnityAction<Ground> OnGround;

        public void OnTile(Ground ground)
        {
            OnGround?.Invoke(ground);
        }

        public void TakeDamage(float damage, Vector3 suspectPos)
        {
            throw new System.NotImplementedException();
        }
    }
}
