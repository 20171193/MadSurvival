using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

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

    public EquipType equipType;
    public ATKType atkType; //� Ÿ�Կ��� ������ ����.
    public abstract void Equip(Player player);
    public abstract void UnEquip(Player player);
}
