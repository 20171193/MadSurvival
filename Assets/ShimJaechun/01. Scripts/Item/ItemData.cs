using Jc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct Level_SpecificItemList
    {
        // 레벨 인덱스에 해당하는 아이템리스트 반환
        public List<DropItem> dropItems;

        public Level_SpecificItemList(List<DropItem> dropItems)
        {
            this.dropItems = dropItems;
        }
    }

    public class ItemData : ScriptableObject
    {
        [Header("레벨 별 드랍 아이템")]
        public List<Level_SpecificItemList> level_SpecificItemLists = new List<Level_SpecificItemList>();
    }
}
