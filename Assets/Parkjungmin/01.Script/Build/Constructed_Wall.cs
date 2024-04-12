using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    public class Constructed_Wall : PooledObject, ITileable
    {
        [SerializeField]
        private Ground onGround;
        [Header("벽의 체력")]
        public int maxHp;
        public int ownHp;

        public int OwnHp
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

        public override void Release()
        {
            ownHp = maxHp;
            onGround.type = GroundType.Buildable;
            onGround = null;
            base.Release();
        }
    }
}

