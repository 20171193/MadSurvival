using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jungmin
{
    // ����õ ���� ��� -> Construct
    public class Constructed_Wall : Construct, ITileable, IDamageable
    {
        [SerializeField]
        private Ground onGround;
        [Header("���� ü��")]
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
            // ����õ �߰�
            // ����/�������� ��ϵ� �Լ� ����
            // ��ã�⿡ Ȱ��
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

