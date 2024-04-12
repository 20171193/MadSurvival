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

        public void OnTile(Ground ground)
        {
            onGround = ground;
            onGround.type = GroundType.Wall;
        }

        public override void Release()
        {
            onGround.type = GroundType.Buildable;
            onGround = null;
            base.Release();
        }
    }
}

