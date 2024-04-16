using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using Jc;
using UnityEngine.Events;
public class Armor : Equip_Item
{
    public Armor(ItemData itemdata_) : base(itemdata_)
    {
    }
    [Header("������ ����")]
    public int level;

    [Header("������ ��� ��")]
    [SerializeField] float def; //����

    public override void Equip(Player player)
    {

        if (player.Stat.AMR == 10)
        {
            player.Stat.AMR += def;
        }

    }
    public override void UnEquip(Player player)
    {
        player.Stat.AMR -= def;
    }

}
