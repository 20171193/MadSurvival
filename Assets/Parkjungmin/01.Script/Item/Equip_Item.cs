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
        [SerializeField]
        private int durable; //�������� ������
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

        [Header("���� ����")]
        public int Weapon_Level;

        public EquipType equipType;
        public ATKType atkType; //� Ÿ�Կ��� ������ ����.
        
        public abstract void Equip(Player player);
        public abstract void UnEquip(Player player);

        private void Start()
        {
            
        }


        public Equip_Item(ItemData itemdata_) : base(itemdata_) { }
    }
}
