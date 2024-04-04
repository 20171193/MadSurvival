using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Experimental.GraphView;
namespace Jc
{
    public enum DirectionType
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public struct Direction
    {
        public int z;
        public int x;
        public Direction(int z, int x)
        {
            this.z = z;
            this.x = x;
        }
    }

    public class NavigationManager : Singleton<NavigationManager>
    {
        // 전체 게임맵
        public List<GroundList> gameMap;

        // 좌표별 맵의 크기
        public int mapZsize;
        public int mapXsize;

        // 플레이어가 벽을 지을 수 있는 공간은 9분할된 맵의 가운데 공간
        // 해당 가운데 공간(정사각형)의 각 모서리 좌표
        public GroundPos cornerTL;  // 좌상단 좌표
        public GroundPos cornerTR;  // 우상단 좌표
        public GroundPos cornerBL;  // 좌하단 좌표
        public GroundPos cornerBR;  // 우하단 좌표

        // 플레이어가 위치한 타일이 변경되었을 때 발생할 액션
        // 몬스터에서 함수를 등록
        public UnityAction<Ground> OnChangePlayerGround;

        // 플레이어가 위치한 좌표의 그라운드
        // 임의 변경이 불가하고 함수 호출로 변경
        [SerializeField]
        private Ground onPlayerGround;
        public Ground OnPlayerGround { get { return onPlayerGround; } }

        private Direction upTile = new Direction(1, 0);
        private Direction downTile = new Direction(-1, 0);
        private Direction leftTile = new Direction(0, -1);
        private Direction rightTile = new Direction(0, 1);

        public void AssginGameMap(List<GroundList> gameMap)
        {
            this.gameMap = gameMap;
        }
        public void EnterPlayerGround(Ground target)
        {
            onPlayerGround = target;
            // 길찾기를 실시하고있는 몬스터, 동물들이 목표지점을 변경해야 함.
            OnChangePlayerGround?.Invoke(target);
        }
        /// <summary>
        /// 현재 위치의 타일과 다음 위치를 받아 다음 위치의 타일을 리턴
        /// </summary>
        /// <param name="curGround"></param>
        /// 현재 위치한 타일
        /// <param name="dirType"></param>
        /// 다음 위치를 설정할 방향타입
        /// <returns></returns>
        public Ground GetGround(Ground curGround, DirectionType dirType)
        {
            Direction dir;
            switch (dirType)
            {
                case DirectionType.UP:
                    dir = upTile;
                    break;
                case DirectionType.DOWN:
                    dir = downTile;
                    break;
                case DirectionType.LEFT:
                    dir = leftTile;
                    break;
                case DirectionType.RIGHT:
                    dir = rightTile;
                    break;
                default:
                    dir = upTile;
                    break;
            }
            int nz = dir.z + curGround.Pos.z;
            int nx = dir.x + curGround.Pos.x;
            // 맵을 벗어난 경우
            if (nz < 0 || nz >= mapZsize || nx < 0 || nx >= mapXsize)
                return null;

            return gameMap[nz].groundList[nx];
        }
    }
}
