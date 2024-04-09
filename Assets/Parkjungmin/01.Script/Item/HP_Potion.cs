using Jc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [Serializable]
    public class HP_Potion : Used_Item
    {
        public HP_Potion(ItemData itemdata_) : base(itemdata_) { }

        [SerializeField] float IncreaseValue;

        public override void Use(Player player)
        {
            player.Stat.OwnHp += IncreaseValue;
        }
    }
}
