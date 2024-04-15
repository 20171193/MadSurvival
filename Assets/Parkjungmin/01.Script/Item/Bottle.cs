using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jungmin
{
    [CreateAssetMenu(fileName = "Stamina_Potion", menuName = "Item/Used_Item/Potion/Stamina_Potion")]
    public class Bottle : Used_Item
    {
        [SerializeField] int maxCapacity; //최대 용량
        [SerializeField] public int ownCapacity; //현재 용량
        [SerializeField] int usePerValue; //한번 마실 때 갈증이 해소되는 값.

        public UnityAction OnUseBottle;

        public Bottle(ItemData itemdata_) : base(itemdata_) { }
        public override void Use(Player player)
        {
            // 용량이 있다면
            if (ownCapacity >= usePerValue)
            {
                if (!player.IsOnWater)
                {

                    player.Stat.OwnThirst += usePerValue;
                    ownCapacity -= usePerValue;
                    OnUseBottle();
                    // 심재천 수정
                    ScoreboardInvoker.Instance.drinkWater?.Invoke(ScoreType.Water);
                }
                else
                {
                    ownCapacity = maxCapacity;
                    OnUseBottle();
                }
            }
            else //용량이 없다면
            {
                if (!player.IsOnWater)
                {
                    return;
                }

                    ownCapacity = maxCapacity;
                    OnUseBottle();
            }

        }
    }

}
