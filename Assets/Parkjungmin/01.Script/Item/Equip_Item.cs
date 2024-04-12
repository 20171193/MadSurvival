using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.Events;

namespace jungmin
{
    public abstract class Equip_Item : Item  //���� ��� ������.
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

        [Header("����� ������")]
        public int durable = 100; //�������� ������
        [SerializeField] public float DecDurableValue;
        public EquipType equipType;
        public ATKType atkType; //� Ÿ�Կ��� ������ ����.
        public UnityAction OnUse;
        
        public abstract void Equip(Player player);
        public abstract void UnEquip(Player player);
        


        public Equip_Item(ItemData itemdata_) : base(itemdata_) { }
    }
}
