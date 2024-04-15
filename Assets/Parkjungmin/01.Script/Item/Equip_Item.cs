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
        [SerializeField]
        private int durable; //아이템의 내구도
        public int Durable
        {
            get { return durable; }
            set
            {
                durable = value;
                OnUse?.Invoke();
            }
        }

        public UnityAction OnUse;

        [Header("무기 레벨")]
        public int Weapon_Level;

        public EquipType equipType;
        public ATKType atkType; //어떤 타입에게 공격을 할지.
        
        public abstract void Equip(Player player);
        public abstract void UnEquip(Player player);

        private void Start()
        {
            
        }


        public Equip_Item(ItemData itemdata_) : base(itemdata_) { }
    }
}
