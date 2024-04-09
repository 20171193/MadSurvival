using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace jungmin
{
    public class Build_Base : Item
    {
        [SerializeField] GameObject bulidPrefab;
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
            Instantiate(bulidPrefab);
        }

    }
}
