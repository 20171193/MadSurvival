using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

namespace jungmin
{
    public abstract class Used_Item : Item
    {
        public bool isInfinite; 
        // 이 아이템을 사용해도 인벤토리에서 없어지지 않음 여부.
        
        public Used_Item(ItemData itemdata_) : base(itemdata_){}
        public abstract void Use(Player player);

    }
}
