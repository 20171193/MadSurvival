using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Jc
{
    /**************************************************************************************
    *** �׶���(���Ӹ� �� �񰡽����� Ÿ��)�� ��ȣ�ۿ� �� �� �ִ� ������Ʈ�� ũ�� 4����
     * Buildable : 1. �浹ü�� �������� ����.
     *             2. �÷��̾ ���� ���� �� �ִ� Ÿ��.
     *             3. �÷��̾ ������ �ѷ����� �� �Ǵ��� ������ �� Ÿ��
     *
     *    Object : 1. �浹ü�� �����Ͽ� �հ� ������ �� ����.
     *             2. �÷��̾�Դ� ä�� ���.
     *             3. ���� ���� �� ������.
     *             4. �ı��� ��� �����ð� ������ ���� ������ ��ġ�� ������. 
     *          
     *      Wall : 1. �浹ü�� �����Ͽ� �հ� ������ �� ����.  
     *             2. �÷��̾ ��ġ�ϴ� ��
     *             3. ���Ϳ��Դ� ���ݴ���� �� �� ����.
     *          
     *     Water : 1. �浹ü�� �����Ͽ� �հ� ������ �� ����.
     *             2. ���� ���۽� �����Ǹ� �ı����� ����.      
     *          
     * Empty�� ����, ����, �÷��̾ �����ٴ� �� �ִ� ��.
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
        // ���Ӹ� �� Ÿ���� ��ǩ��
        // ���� ��� : (0, 0)
        // ���� �ϴ� : (Ÿ���� �� ����-1, Ÿ���� �� ���� -1)
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

        // �ǹ��� ���� ���
        public void OnBuild()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                // �÷��̾ ���� ��ġ�� Ÿ���� ��ã�� �����ڿ� �Ҵ�
                // ��ã�� �����ڿ��� �÷��̾ Ž���ϰ��ִ� ��� ���� �׼��� ����
                Manager.Navi.EnterPlayerGround(this);
            }

            // Ÿ�Ͽ� ��ġ�� �� �ִ� ������Ʈ�� ���� Ÿ���� �Ҵ�
            ITileable obj = other.GetComponent<ITileable>();
            if (obj == null) return;
            else obj.OnTile(this);
        }
    }
}
