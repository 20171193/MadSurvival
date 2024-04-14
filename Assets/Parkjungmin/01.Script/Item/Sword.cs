using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    public class Sword : Equip_Item
    {
        [Header("���� ����")]
        [SerializeField] int Weapon_Level;
        [Header("�� ���ݷ�")]
        [SerializeField] float atk; //���ݷ�

        public override void Equip(Player player)
        {
            player.Stat.MonsterATK += atk;
        }

        public override void UnEquip(Player player)
        {
            player.Stat.MonsterATK -= atk;
        }
        public Sword(ItemData itemdata_) : base(itemdata_) { }
    }
}
