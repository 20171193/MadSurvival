using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Jc
{
    /**************************************************************************************
    *** �׶���(���Ӹ� �� �񰡽����� Ÿ��)�� ��ȣ�ۿ� �� �� �ִ� ������Ʈ�� ũ�� 3����
     * Object : 1. �浹ü�� �����Ͽ� �հ� ������ �� ����.
     *          2. �÷��̾�Դ� ä�� ���.
     *          3. ���� ���� �� ������.
     *          4. �ı��� ��� �����ð� ������ ���� ������ ��ġ�� ������. 
     *          
     *   Wall : 1. �浹ü�� �����Ͽ� �հ� ������ �� ����.  
     *          2. �÷��̾ ��ġ�ϴ� ��
     *          3. ���Ϳ��Դ� ���ݴ���� �� �� ����.
     *          
     *  Water : 1. �浹ü�� �����Ͽ� �հ� ������ �� ����.
     *          2. ���� ���۽� �����Ǹ� �ı����� ����.      
     *          
     * Empty�� ����, ����, �÷��̾ �����ٴ� �� �ִ� ��.
     * ���� Ÿ�Կ� ���� ����, ������ A* ����ġ�� ����.
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
        // ���Ӹ� �� Ÿ���� ��ǩ��
        // ���� ��� : (0, 0)
        // ���� �ϴ� : (Ÿ���� �� ����-1, Ÿ���� �� ���� -1)
        [SerializeField]
        private GroundPos pos;
        public GroundPos Pos { get { return pos; } }

        private void OnEnable()
        {
            pos = new GroundPos((int)(gameObject.name[0] - '0'), (int)(gameObject.name[1] - '0'));
        }

        // �ǹ��� ���� ���
        public void OnBuild()
        {

        }

        // �÷��̾ Ÿ�Ͽ� ������ ���
        public void EnterPlayer()
        {
        }

        // ITrackable�� ���� ��,
        // �߰��� �� �ִ� ���� Ȥ�� ���Ͱ� ������ ���
        public void EnterTracker(ITrackable tracker)
        {
            // ���� ��ġ�� Ÿ���� ����
            tracker.OnGround(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
