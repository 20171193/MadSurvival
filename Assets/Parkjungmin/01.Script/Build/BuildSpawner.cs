using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    public class BuildSpawner : MonoBehaviour
    {

        [SerializeField]
        public Constructed_Turret turret;
        public Constructed_Turret turret2;
        public Constructed_Turret turret3;
        public Constructed_Wall fence;
        public Constructed_Wall fence2;
        public Constructed_Wall fence3;
        public Constructed_Wall boneFire;
        public Constructed_Wall boneFire2;


        private void Awake()
        {
            Manager.Pool.CreatePool(turret, turret.Size, turret.Size + 5);
            Manager.Pool.CreatePool(turret2, turret2.Size, turret.Size + 5);
            Manager.Pool.CreatePool(turret3, turret.Size, turret.Size + 5);
            Manager.Pool.CreatePool(fence, fence.Size, fence.Size + 5);
            Manager.Pool.CreatePool(fence2, fence2.Size, fence2.Size + 5);
            Manager.Pool.CreatePool(fence3, fence3.Size, fence3.Size + 5);
            Manager.Pool.CreatePool(boneFire, boneFire.Size, boneFire.Size + 5);
            Manager.Pool.CreatePool(boneFire2, boneFire2.Size, boneFire2.Size + 5);
        }
    }
}
