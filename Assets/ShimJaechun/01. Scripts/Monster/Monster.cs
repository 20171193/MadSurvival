using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Monster : MonoBehaviour
    {
        // ��ü���� ���� ��
        [SerializeField]
        private List<GroundList> gameMap;

        // ���Ͱ� ������ Ÿ��
        public Ground onGround;

        private void OnEnable()
        {
            gameMap = Manager.Navi.gameMap;
        }

        public void OnChangeTarget(Ground playerGround)
        {

        }

        // A* ��ã�� �˰���
        private Ground PathFinding()
        {
            // ����ó�� : ��ã�⸦ ������ ��ġ�� �����Ǿ����� ���� ���
            if(onGround == null)
            {
                Debug.Log($"{this.gameObject.name} : ��ã�� ����");
                return null;
            }

            List<GroundList> gameMap = Manager.Navi.gameMap;

        }
    }
}
