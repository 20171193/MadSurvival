using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

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

    public EquipType equipType;
    public ATKType atkType; //어떤 타입에게 공격을 할지.
    public abstract void Equip(Player player);
    public abstract void UnEquip(Player player);
}
