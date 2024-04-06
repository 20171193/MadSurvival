using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "Axe", menuName = "Item/EquipItem/Axe")]
    public class Axe : Equip_Item //µµ³¢
    {
        [Header("µµ³¢ °ø°Ý·Â")]
        [SerializeField] float atk; //°ø°Ý·Â
        public Axe(ItemData itemdata_) : base(itemdata_) { }
        public override void Equip(Player player)
        {
            player.Stat.MonsterATK += atk;
        }

        public override void UnEquip(Player player)
        {
            player.Stat.MonsterATK -= atk;
        }
    }
}
