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
        // ��ü ���Ӹ�
        public List<GroundList> gameMap;

        // ��ǥ�� ���� ũ��
        public int mapZsize;
        public int mapXsize;

        // �÷��̾ ���� ���� �� �ִ� ������ 9���ҵ� ���� ��� ����
        // �ش� ��� ����(���簢��)�� �� �𼭸� ��ǥ
        public GroundPos cornerTL;  // �»�� ��ǥ
        public GroundPos cornerTR;  // ���� ��ǥ
        public GroundPos cornerBL;  // ���ϴ� ��ǥ
        public GroundPos cornerBR;  // ���ϴ� ��ǥ

        // �÷��̾ ��ġ�� Ÿ���� ����Ǿ��� �� �߻��� �׼�
        // ���Ϳ��� �Լ��� ���
        public UnityAction<Ground> OnChangePlayerGround;

        // �÷��̾ ��ġ�� ��ǥ�� �׶���
        // ���� ������ �Ұ��ϰ� �Լ� ȣ��� ����
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
            // ��ã�⸦ �ǽ��ϰ��ִ� ����, �������� ��ǥ������ �����ؾ� ��.
            OnChangePlayerGround?.Invoke(target);
        }
        /// <summary>
        /// ���� ��ġ�� Ÿ�ϰ� ���� ��ġ�� �޾� ���� ��ġ�� Ÿ���� ����
        /// </summary>
        /// <param name="curGround"></param>
        /// ���� ��ġ�� Ÿ��
        /// <param name="dirType"></param>
        /// ���� ��ġ�� ������ ����Ÿ��
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
            // ���� ��� ���
            if (nz < 0 || nz >= mapZsize || nx < 0 || nx >= mapXsize)
                return null;

            return gameMap[nz].groundList[nx];
        }
    }
}
