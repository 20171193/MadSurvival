using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using Jc;

namespace jungmin
{
    public class Build_Base : Item //�������� ���������� ������ �� ������
    {
        [SerializeField] PooledObject bulidPrefab; //���� �������� �Ǽ� �� ������.
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }
        [Header("�������� ü��")]
        [SerializeField] public float building_hp;

        public void Build(Ground socketGround)
        {
            Debug.Log($"Build : {socketGround}");
            //GameObject building = Instantiate(bulidPrefab, socketGround.transform.position, Quaternion.identity);
            Manager.Pool.GetPool(bulidPrefab, socketGround.transform.position, Quaternion.identity);
            socketGround.type = GroundType.Wall;
        }

    }
}
