using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    public class Constructed_Wall : PooledObject, ITileable, IDamageable
    {
        [SerializeField]
        private Ground onGround;
        [Header("���� ü��")]
        public float maxHp;
        public float ownHp;

        public float OwnHp
        {
            get
            {
                return ownHp;
            }
            set
            {
                ownHp = value;

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
            ownHp = maxHp;
            onGround.type = GroundType.Buildable;
            onGround = null;
            base.Release();
        }

        public void TakeDamage(float damage)
        {
            OwnHp -= damage;
        }
    }
}

