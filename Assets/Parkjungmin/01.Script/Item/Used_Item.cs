using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

namespace jungmin
{
    public abstract class Used_Item : Item
    {
        public Used_Item(ItemData itemdata_) : base(itemdata_){}
        public abstract void Use(Player player);

    }
}
