using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "HP_Potion", menuName = "Item/Used_Item/Potion/HP_Potion")]
    public class HP_Potion : Used_Item
    {
        [SerializeField] float IncreaseValue;

        public override void Use(Player player)
        {
            player.Stat.OwnHp += IncreaseValue;
        }
    }
}
