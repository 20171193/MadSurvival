using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using Jc;

namespace jungmin
{
    public class Build_Base : Item //구조물이 아이템으로 존재할 때 데이터
    {
        [SerializeField] PooledObject bulidPrefab; //실제 구조물을 건설 시 프리팹.
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }
        [Header("구조물의 체력")]
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
