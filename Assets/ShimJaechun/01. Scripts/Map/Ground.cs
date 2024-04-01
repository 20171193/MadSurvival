using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Jc
{
    /**************************************************************************************
    *** 그라운드(게임맵 상 비가시적인 타일)와 상호작용 할 수 있는 오브젝트는 크게 4종류
     * Buildable : 1. 충돌체가 존재하지 않음.
     *             2. 플레이어가 벽을 지을 수 있는 타일.
     *             3. 플레이어가 벽으로 둘러싸인 지 판단할 기준이 될 타일
     *
     *    Object : 1. 충돌체가 존재하여 뚫고 지나갈 수 없음.
     *             2. 플레이어에게는 채굴 대상.
     *             3. 게임 시작 시 생성됨.
     *             4. 파괴된 경우 일정시간 딜레이 이후 랜덤한 위치에 생성됨. 
     *          
     *      Wall : 1. 충돌체가 존재하여 뚫고 지나갈 수 없음.  
     *             2. 플레이어가 설치하는 벽
     *             3. 몬스터에게는 공격대상이 될 수 있음.
     *          
     *     Water : 1. 충돌체가 존재하여 뚫고 지나갈 수 없음.
     *             2. 게임 시작시 생성되며 파괴되지 않음.      
     *          
     * Empty는 몬스터, 동물, 플레이어가 지나다닐 수 있는 길.
    ****************************************************************************************/
    public enum GroundType
    {
        Empty,
        Buildable,
        Object,
        Wall,
        Water
    }
    [Serializable]
    public struct GroundPos
    {
        public int z;
        public int x;
        public GroundPos(int z, int x)
        {
            this.z = z;
            this.x = x;
        }
    }
    public class Ground : MonoBehaviour
    {
        // 게임맵 상 타일의 좌푯값
        // 좌측 상단 : (0, 0)
        // 우측 하단 : (타일의 총 개수-1, 타일의 총 개수 -1)
        [SerializeField]
        private GroundPos pos;
        public GroundPos Pos { get { return pos; } }

        [SerializeField]
        public GroundType type;

        private void OnEnable()
        {
            string[] position = gameObject.name.Split(',');
            pos = new GroundPos(int.Parse(position[0]), int.Parse(position[1]));
        }

        // 건물을 짓는 경우
        public void OnBuild()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                // 플레이어가 현재 위치한 타일을 길찾기 관리자에 할당
                // 길찾기 관리자에서 플레이어를 탐색하고있는 모든 몬스터 액션을 실행
                Manager.Navi.EnterPlayerGround(this);
            }

            // 타일에 위치할 수 있는 오브젝트에 현재 타일을 할당
            ITileable obj = other.GetComponent<ITileable>();
            if (obj == null) return;
            else obj.OnTile(this);
        }
    }
}
