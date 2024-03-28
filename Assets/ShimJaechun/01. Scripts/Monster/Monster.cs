using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Monster : MonoBehaviour, ITrackable
    {
        // ��ü���� ���� ��
        [SerializeField]
        private List<GroundList> gameMap;

        // ���Ͱ� ������ Ÿ��
        public Ground onGround;

        private Ground playerGround;

        private void OnEnable()
        {
            // �÷��̾��� Ÿ�� ��ġ�� ����� ��� �߻��� �׼�
            Manager.Navi.OnChangePlayerGround += OnChangeTarget;
            // ���Ӹ� �Ҵ�
            gameMap = Manager.Navi.gameMap;
        }
        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // ���Ͱ� Ÿ�Ͽ� ������ ��� ����
        public void OnGround(Ground ground)
        {
            onGround = ground;
        }

        public void OnChangeTarget(Ground playerGround)
        {

        }
        #region ��ã�� �˰��� ����
        /*****************************************************************************************************************
         * 
         *  1. �÷��̾ ������ �ѷ��ο� �ִ��� üũ (�÷��̾� �������� BFSŽ��)
         *    a. ������ �ѷ��ο��ִٸ� 
         *     a-1. ���� ��ġ���� ���� ����� ���� ã�� Astar �˰��� ����
         *     a-2. 3�ʿ� �ѹ��� 1�� ����� (���� �� ���� �ı��ϰų�, ���� �����ϴ� ��찡 �����ϱ� ����)
         *     
         *    b. �÷��̾�� ������ �� �ִٸ�
         *     b-1. �÷��̾ ��ġ�� Ÿ�Ϸ� Astar �˰��� ����
         *     b-2. 3�ʿ� �ѹ��� 1�� �����
         *  
         *  2. ���������� ��ǥ������ ������ ��� 
         *    2-1. Ÿ���� ���� ����
         *     2-1-a. Ÿ���� �÷��̾��� ��� (������ �� ����). 1�� �����.
         *     2-1-b. Ÿ���� ���� ��� (������ �� ����). 3�ʿ� �ѹ��� 1�� �����.
         * 
         * �߰�)
         * ���� ���� �� �ִ� ������ �����Ǿ����� ��� BFS�� ȿ������ �� ����.
         * 60x60���� 9������ �� ��� ����(�÷��̾ ���� ���� �� �ִ� ����)�� �÷��̾ ��ġ�� ��쿡�� BFSŽ���� �ǽ�.
         *  - BFS Ž���� �־ǿ��� ������ ������ ���� ��ü�� Ž���ؾ�������, N^2 
         *  ���� Ư���� ���� ���� �� �ִ� ������ 20*20�̹Ƿ� (N/9)^2 ���� ���� ���� �� ���� �� ����.
         *  
         *  �� ���� ����ȭ�� �ʿ��ϴٸ� �÷��̾� �������� ����ĳ��Ʈ�� Ȱ���ϴ� ����� ���.
         *   - �� ����� ���� ���̰� �� ������ �� �־� ���̰� ���� �ʴ� ��찡 �߻��� �� �����Ƿ� ������ �������� ����.
         *    ? ���� ��� �� �� ����.
         *****************************************************************************************************************/
        #endregion

        // A* ��ã�� �˰���
        private Ground PathFinding()
        {
            // ����ó�� : ��ã�⸦ ������ ��ġ/���� �����Ǿ����� ���� ���
            if(onGround == null || playerGround == null ||  gameMap.Count < 1)
            {
                Debug.Log($"{this.gameObject.name} : ��ã�� ����");
                Debug.Log($"���� : {onGround}, �÷��̾� : {playerGround}, �� : {gameMap}");
                return null;
            }
            // �۾� �ߴ� �κ�
            return null;
        }

        // �÷��̾�� ������ �� �ִ��� üũ
        private bool IsReachable()
        {
            // �۾� �ߴ� �κ�
            return false;
        }

        private void Astar()
        {

        }
    }
}
