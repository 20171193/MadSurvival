using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public interface IDamageable
    {
        // �������� �������� ���� ����� ��ġ 
        public void TakeDamage(float damage, Vector3 suspectPos);
    }
}
