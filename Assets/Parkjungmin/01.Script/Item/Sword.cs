using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    public class Sword : Equip_Item
    {
        [Header("검 공격력")]
        [SerializeField] float atk; //공격력

        public override void Equip(Player player)
        {
            player.Stat.MonsterATK += atk;
            this.durable -= DecDurableValue;
            Debug.Log($"{durable}");
            if (this.durable <= 0)
            {
                player.UnEquip(EquipType.Weapon);
            }
        }

        public override void UnEquip(Player player)
        {
            player.Stat.MonsterATK -= atk;
        }
        public Sword(ItemData itemdata_) : base(itemdata_) { }
    }
}
