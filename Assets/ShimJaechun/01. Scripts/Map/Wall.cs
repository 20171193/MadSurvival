using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Wall : MonoBehaviour, ITileable
    {
        [Header("Specs")]
        private float amr;
        private float hp; 

        private Ground onGround;
        public Ground OnGround { get { return onGround; } } 

        public void OnTile(Ground ground)
        {
            onGround = ground;
        }
    }
}
