using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 심재천 추가
// 터렛, 벽 등의 상위 클래스
namespace Jc
{
    public class Construct : PooledObject
    {
        // 심재천 추가
        public UnityAction<GameObject> OnDestroyWall;
    }
}
