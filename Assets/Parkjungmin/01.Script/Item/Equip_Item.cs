using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.Events;

namespace jungmin
{
    public abstract class Equip_Item : Item  //무기 장비 아이템.
    {
        public enum ATKType
        {
            None,
            Monster,
            Tree,
            Stone
        }

        public enum EquipType
        {
            Weapon,
            Armor
        }

        [Header("장비의 내구도")]
        public int durable = 100; //아이템의 내구도
        [SerializeField] public float DecDurableValue;
        public EquipType equipType;
        public ATKType atkType; //어떤 타입에게 공격을 할지.
        public UnityAction OnUse;
        
        public abstract void Equip(Player player);
        public abstract void UnEquip(Player player);
        


        public Equip_Item(ItemData itemdata_) : base(itemdata_) { }
    }
}
