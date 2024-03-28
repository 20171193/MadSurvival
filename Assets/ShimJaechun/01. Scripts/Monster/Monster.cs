using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Monster : MonoBehaviour
    {
        // 전체적인 게임 맵
        [SerializeField]
        private List<GroundList> gameMap;

        // 몬스터가 스폰된 타일
        public Ground onGround;

        private void OnEnable()
        {
            gameMap = Manager.Navi.gameMap;
        }

        public void OnChangeTarget(Ground playerGround)
        {

        }

        // A* 길찾기 알고리즘
        private Ground PathFinding()
        {
            // 예외처리 : 길찾기를 시작할 위치가 지정되어있지 않은 경우
            if(onGround == null)
            {
                Debug.Log($"{this.gameObject.name} : 길찾기 오류");
                return null;
            }

            List<GroundList> gameMap = Manager.Navi.gameMap;

        }
    }
}
