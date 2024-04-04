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
        // ���� �ε����� �ش��ϴ� �����۸���Ʈ ��ȯ
        public List<DropItem> dropItems;

        public Level_SpecificItemList(List<DropItem> dropItems)
        {
            this.dropItems = dropItems;
        }
    }

    public class ItemData : ScriptableObject
    {
        [Header("���� �� ��� ������")]
        public List<Level_SpecificItemList> level_SpecificItemLists = new List<Level_SpecificItemList>();
    }
}
