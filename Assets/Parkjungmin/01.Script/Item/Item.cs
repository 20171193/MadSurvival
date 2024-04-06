using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    public class Item : MonoBehaviour
    {

        public Item(ItemData itemdata_)
        {
            itemdata = itemdata_;
        }

        [SerializeField] public ItemData itemdata;
    }
}
