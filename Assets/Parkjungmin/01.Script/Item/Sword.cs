using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Item/EquipItem/Sword")]
public class Sword : Equip_Item
{
    [Header("검 공격력")]
    [SerializeField] float atk; //공격력

    public override void Equip(Player player)
    {
        player.Stat.MonsterATK += atk;
    }

    public override void UnEquip(Player player)
    {
        player.Stat.MonsterATK -= atk;
    }
}
