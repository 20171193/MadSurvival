using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

namespace Jc
{
    public interface ITileable
    {
        // Ÿ�� ���� ��ġ�� �� �ִ� ������Ʈ�鿡 �Ҵ�.
        public void OnTile(Ground ground);
        public Ground GetOnTile();
    }
}
