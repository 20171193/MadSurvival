using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using Jc;
public class Armor : Equip_Item
{
    public Armor(ItemData itemdata_) : base(itemdata_)
    {
    }
    [Header("°©¿ÊÀÇ ¹æ¾î °ª")]
    [SerializeField] float def; //¹æ¾î·Â
    public override void Equip(Player player)
    {
        player.Stat.AMR += def;
    }

    public override void UnEquip(Player player)
    {
        player.Stat.AMR -= def;
    }
}
