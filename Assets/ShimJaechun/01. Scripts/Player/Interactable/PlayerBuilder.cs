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
            // 현재 타일의 위치에서 플레이어의 앞방향 타일에 빌딩 소켓을 지정
            // 앞 방향 타일이 지을 수 있는 타일일 경우 소켓 타일로 지정
            socketGround = CheckFrontGround(onGround, transform);
        }

        private Ground CheckFrontGround(Ground onGround, Transform transform)
        {
            // 플레이어가 바라보고있는 방향에 따라 위쪽 타일을 설정

            return null;
        }
    }
}
