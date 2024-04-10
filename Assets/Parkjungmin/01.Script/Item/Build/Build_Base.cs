using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace jungmin
{
    public class Build_Base : Item //구조물이 아이템으로 존재할 때 데이터
    {
        [SerializeField] GameObject bulidPrefab; //실제 구조물을 건설 시 프리팹.
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }
        [SerializeField] bool Isbuild;
        [SerializeField] public float building_hp;


        private void Update()
        {
            if (Isbuild)
            {
                Build();
            }
        }
        public void Build()
        {
            
            GameObject building = Instantiate(bulidPrefab);
         
        }

    }
}
