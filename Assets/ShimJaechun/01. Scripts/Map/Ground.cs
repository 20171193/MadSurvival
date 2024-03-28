using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Jc
{
    /**************************************************************************************
    *** 그라운드(게임맵 상 비가시적인 타일)와 상호작용 할 수 있는 오브젝트는 크게 3종류
     * Object : 1. 충돌체가 존재하여 뚫고 지나갈 수 없음.
     *          2. 플레이어에게는 채굴 대상.
     *          3. 게임 시작 시 생성됨.
     *          4. 파괴된 경우 일정시간 딜레이 이후 랜덤한 위치에 생성됨. 
     *          
     *   Wall : 1. 충돌체가 존재하여 뚫고 지나갈 수 없음.  
     *          2. 플레이어가 설치하는 벽
     *          3. 몬스터에게는 공격대상이 될 수 있음.
     *          
     *  Water : 1. 충돌체가 존재하여 뚫고 지나갈 수 없음.
     *          2. 게임 시작시 생성되며 파괴되지 않음.      
     *          
     * Empty는 몬스터, 동물, 플레이어가 지나다닐 수 있는 길.
     * 위의 타입에 따라 몬스터, 동물의 A* 가중치를 설정.
    ****************************************************************************************/
    public enum GroundType
    {
        Empty,
        Object,
        Wall,
        Water
    }
    [Serializable]
    public struct GroundPos
    {
        public int x;
        public int y;
        public GroundPos(int x, int y)
        {
            this.x = x;
            this.y = y;
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

        private void OnEnable()
        {
            pos = new GroundPos((int)(gameObject.name[0] - '0'), (int)(gameObject.name[1] - '0'));
        }

        // 건물을 짓는 경우
        public void OnBuild()
        {

        }

        // 플레이어가 타일에 진입한 경우
        public void EnterPlayer()
        {
        }

        // ITrackable을 지닌 즉,
        // 추격할 수 있는 동물 혹은 몬스터가 진입한 경우
        public void EnterTracker(ITrackable tracker)
        {
            // 현재 위치한 타일을 전달
            tracker.OnGround(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
