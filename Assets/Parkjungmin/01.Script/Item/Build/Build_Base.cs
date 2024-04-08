using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace jungmin
{


    public class Build_Base : Item
    {
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }

        [SerializeField] public float building_hp;

    }
}
