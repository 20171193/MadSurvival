using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ����õ �߰�
// �ͷ�, �� ���� ���� Ŭ����
namespace Jc
{
    public class Construct : PooledObject
    {
        // ����õ �߰�
        public UnityAction<GameObject> OnDestroyWall;
    }
}
