using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class TiledObject : MonoBehaviour, ITileable
    {
        // 타일 위에 위치하는 오브젝트
        [Header("빈 상태일 때 타일 타입")]
        [SerializeField]
        protected GroundType originType;

        [Space(2)]
        [Header("오브젝트가 위치한 경우의 타일 타입")]
        [SerializeField]
        protected GroundType groundType;

        [Space(2)]
        [Header("현재 오브젝트가 위치한 타일")]
        [SerializeField]
        protected Ground onGround;

        public virtual void OnTile(Ground ground)
        {
            onGround = ground;
            ChangeGroundType(true);
        }
        public Ground GetOnTile()
        {
            return onGround;
        }
        private void ChangeGroundType(bool isEnable)
        {
            //// 게임맵에서 현재 오브젝트가 위치한 타일의 타입을 변경
            //Manager.Navi.gameMap[onGround.Pos.z].groundList[onGround.Pos.x].type
            //    = isEnable ? groundType : originType;
        }
        protected virtual void OnDisable()
        {
            ChangeGroundType(false);
        }
    }
}
