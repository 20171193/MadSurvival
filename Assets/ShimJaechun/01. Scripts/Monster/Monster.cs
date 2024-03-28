using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Monster : MonoBehaviour, ITrackable
    {
        // 전체적인 게임 맵
        [SerializeField]
        private List<GroundList> gameMap;

        // 몬스터가 스폰된 타일
        public Ground onGround;

        private Ground playerGround;

        private void OnEnable()
        {
            // 플레이어의 타일 위치가 변경될 경우 발생할 액션
            Manager.Navi.OnChangePlayerGround += OnChangeTarget;
            // 게임맵 할당
            gameMap = Manager.Navi.gameMap;
        }
        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // 몬스터가 타일에 진입한 경우 세팅
        public void OnGround(Ground ground)
        {
            onGround = ground;
        }

        public void OnChangeTarget(Ground playerGround)
        {

        }
        #region 길찾기 알고리즘 로직
        /*****************************************************************************************************************
         * 
         *  1. 플레이어가 벽으로 둘러싸여 있는지 체크 (플레이어 기준으로 BFS탐색)
         *    a. 벽으로 둘러싸여있다면 
         *     a-1. 현재 위치에서 가장 가까운 벽을 찾아 Astar 알고리즘 실행
         *     a-2. 3초에 한번씩 1을 재실행 (전투 중 벽을 파괴하거나, 벽을 생성하는 경우가 존재하기 때문)
         *     
         *    b. 플레이어에게 도달할 수 있다면
         *     b-1. 플레이어가 위치한 타일로 Astar 알고리즘 실행
         *     b-2. 3초에 한번씩 1을 재실행
         *  
         *  2. 최종적으로 목표지점에 도달한 경우 
         *    2-1. 타깃을 향해 공격
         *     2-1-a. 타깃이 플레이어인 경우 (움직일 수 있음). 1을 재실행.
         *     2-1-b. 타깃이 벽인 경우 (움직일 수 없음). 3초에 한번씩 1을 재실행.
         * 
         * 추가)
         * 벽을 지을 수 있는 공간이 한정되어있을 경우 BFS가 효과적일 것 같음.
         * 60x60맵을 9분할한 뒤 가운데 공간(플레이어가 벽을 지을 수 있는 공간)에 플레이어가 위치한 경우에만 BFS탐색을 실시.
         *  - BFS 탐색이 최악에는 끝점을 제외한 맵의 전체를 탐색해야하지만, N^2 
         *  게임 특성상 벽을 지을 수 있는 공간은 20*20이므로 (N/9)^2 으로 많이 줄일 수 있을 것 같음.
         *  
         *  이 또한 최적화가 필요하다면 플레이어 기준으로 레이캐스트를 활용하는 방법을 사용.
         *   - 이 방법은 벽의 높이가 제 각각일 수 있어 레이가 닿지 않는 경우가 발생할 수 있으므로 최후의 수단으로 보류.
         *    ? 낮게 쏘면 될 것 같음.
         *****************************************************************************************************************/
        #endregion

        // A* 길찾기 알고리즘
        private Ground PathFinding()
        {
            // 예외처리 : 길찾기를 시작할 위치/맵이 지정되어있지 않은 경우
            if(onGround == null || playerGround == null ||  gameMap.Count < 1)
            {
                Debug.Log($"{this.gameObject.name} : 길찾기 오류");
                Debug.Log($"몬스터 : {onGround}, 플레이어 : {playerGround}, 맵 : {gameMap}");
                return null;
            }
            // 작업 중단 부분
            return null;
        }

        // 플레이어에게 도착할 수 있는지 체크
        private bool IsReachable()
        {
            // 작업 중단 부분
            return false;
        }

        private void Astar()
        {

        }
    }
}
