using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using Jc;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UIElements;

namespace jungmin
{
    public class Build_Base : Item //�������� ���������� ������ �� ������
    {
        [SerializeField] PooledObject bulidPrefab; //���� �������� �Ǽ� �� ������.
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }
        [Header("�������� ü��")]
        [SerializeField] public float building_hp;
        [Header("���� Position ��")]
        [SerializeField] bool hasPos;

        // Method : ������ �Ǽ� ****
        public void Build(Ground socketGround, BuildDirection direction)
        {
            Debug.Log($"Build : {socketGround}");
            if (!hasPos)
                Manager.Pool.GetPool(bulidPrefab, socketGround.transform.position, Quaternion.identity);
            else
            {
                Vector3 Pos = Vector3.zero;
                Vector3 Dir = Vector3.zero;

                switch (direction)
                {
                    case BuildDirection.Front:
                        Pos = new Vector3(socketGround.transform.position.x, 1.2f,socketGround.transform.position.z - 0.3f);
                        Dir = transform.forward;
                        break;
                    case BuildDirection.Back:
                        Pos = new Vector3(socketGround.transform.position.x, 1.2f, socketGround.transform.position.z + 0.3f);
                        Dir = -transform.forward;
                        break;
                    case BuildDirection.Left:
                        Pos = new Vector3(socketGround.transform.position.x + 0.3f, 1.2f, socketGround.transform.position.z);
                        Dir = -transform.right;
                        break;
                    case BuildDirection.Right:
                        Pos = new Vector3(socketGround.transform.position.x-0.3f, 1.2f, socketGround.transform.position.z);
                        Dir = transform.right;
                        break;

                }

                PooledObject inst = Manager.Pool.GetPool(bulidPrefab,Pos,Quaternion.identity);
                inst.transform.forward = Dir;

            }
           
            socketGround.type = GroundType.Wall;
        }

        //  ��
        // �� ��
        //  ��
    }
}
