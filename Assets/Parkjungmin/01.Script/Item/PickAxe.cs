using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "PickAxe", menuName = "Item/EquipItem/PickAxe")]
    public class PickAxe : Equip_Item //°î±ªÀÌ
    {
        [Header("°î±ªÀÌ ¼º´É °ª")]
        [SerializeField] float atk; //°ø°Ý·Â
        public PickAxe(ItemData itemdata_) : base(itemdata_) { }
        public override void Equip(Player player)
        {
            player.Stat.StoneATK += atk;
        }

        public override void UnEquip(Player player)
        {
            player.Stat.StoneATK -= atk;
        }
    }
}
