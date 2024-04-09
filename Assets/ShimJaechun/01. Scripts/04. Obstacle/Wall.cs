using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Wall : TiledObject, IDamageable
    {
        public Ground OnGround { get { return onGround; } }

        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        private float amr;
        [SerializeField]
        private float hp;

        private void Awake()
        {
            groundType = GroundType.Wall;
        }

        public override void OnTile(Ground ground)
        {
            base.OnTile(ground);
        }

        public void TakeDamage(float damage)
        {
        }
    }
}
