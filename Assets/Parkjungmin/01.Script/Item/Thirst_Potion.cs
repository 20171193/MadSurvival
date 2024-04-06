using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "Stamina_Potion", menuName = "Item/Used_Item/Potion/Stamina_Potion")]
    public class Thirst_Potion : Used_Item
    {
        [SerializeField] float staminaIncValue;
        public Thirst_Potion(ItemData itemdata_) : base(itemdata_) { }
        public override void Use(Player player)
        {
            player.Stat.OwnThirst += staminaIncValue;
        }
    }
}