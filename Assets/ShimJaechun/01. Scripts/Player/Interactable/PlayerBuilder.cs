using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerBuilder : MonoBehaviour
    {
        public Ground socketGround = null;

        public void SetBuildGround(Ground onGround, Transform transform)
        {
            // ���� Ÿ���� ��ġ���� �÷��̾��� �չ��� Ÿ�Ͽ� ���� ������ ����
            // �� ���� Ÿ���� ���� �� �ִ� Ÿ���� ��� ���� Ÿ�Ϸ� ����
            socketGround = CheckFrontGround(onGround, transform);
        }

        private Ground CheckFrontGround(Ground onGround, Transform transform)
        {
            // �÷��̾ �ٶ󺸰��ִ� ���⿡ ���� ���� Ÿ���� ����

            return null;
        }
    }
}
