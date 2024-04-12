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
    public class Build_Base : Item //구조물이 아이템으로 존재할 때 데이터
    {
        [SerializeField] PooledObject bulidPrefab; //실제 구조물을 건설 시 프리팹.
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }
        [Header("구조물의 체력")]
        [SerializeField] public float building_hp;
        [Header("고정 Position 값")]
        [SerializeField] bool hasPos;

        // Method : 구조물 건설 ****
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

        //  앞
        // 좌 우
        //  뒤
    }
}
