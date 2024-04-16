using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class TiledObject : MonoBehaviour, ITileable
    {
        // Ÿ�� ���� ��ġ�ϴ� ������Ʈ
        [Header("�� ������ �� Ÿ�� Ÿ��")]
        [SerializeField]
        protected GroundType originType;

        [Space(2)]
        [Header("������Ʈ�� ��ġ�� ����� Ÿ�� Ÿ��")]
        [SerializeField]
        protected GroundType groundType;

        [Space(2)]
        [Header("���� ������Ʈ�� ��ġ�� Ÿ��")]
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
            //// ���Ӹʿ��� ���� ������Ʈ�� ��ġ�� Ÿ���� Ÿ���� ����
            //Manager.Navi.gameMap[onGround.Pos.z].groundList[onGround.Pos.x].type
            //    = isEnable ? groundType : originType;
        }
        protected virtual void OnDisable()
        {
            ChangeGroundType(false);
        }
    }
}
