using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public interface IDamageable
    {
        // 데미지와 데미지를 입힌 상대의 위치 
        public void TakeDamage(float damage, Vector3 suspectPos);
    }
}
