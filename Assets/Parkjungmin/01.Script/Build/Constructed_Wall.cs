using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jungmin
{
    // 심재천 수정 상속 -> Construct
    public class Constructed_Wall : Construct, ITileable, IDamageable
    {
        [SerializeField]
        private Ground onGround;
        [Header("벽의 체력")]
        public float maxHp;
        public float ownHp;
        public AudioSource OnDamagedSound;

        public float OwnHp
        {
            get
            {
                return ownHp;
            }
            set
            {
                ownHp = value;
                OnDamagedSound?.Play();
                if (ownHp <= 0)
                {
                    Release();
                }
            }
        }

        public void OnTile(Ground ground)
        {
            onGround = ground;
            onGround.type = GroundType.Wall;
        }
        public Ground GetOnTile()
        {
            return onGround;
        }
        public override void Release()
        {
            // 심재천 추가
            // 몬스터/동물에서 등록된 함수 실행
            // 길찾기에 활용
            OnDestroyWall?.Invoke(gameObject);

            ownHp = maxHp;
            onGround.SetOriginType();
            onGround = null;
            base.Release();
        }

        public void TakeDamage(float damage)
        {
            OwnHp -= damage;
        }
    }
}

