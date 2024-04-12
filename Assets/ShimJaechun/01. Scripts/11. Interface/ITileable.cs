using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

namespace Jc
{
    public interface ITileable
    {
        // 타일 위에 위치할 수 있는 오브젝트들에 할당.
        public void OnTile(Ground ground);
        public Ground GetOnTile();
    }
}
