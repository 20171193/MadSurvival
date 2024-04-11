using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "Stamina_Potion", menuName = "Item/Used_Item/Potion/Stamina_Potion")]
    public class Bottle : Used_Item
    {
        [SerializeField] float maxCapacity; //최대 용량
        [SerializeField] float ownCapacity; //현재 용량

        [SerializeField] float usePerValue; //한번 마시는 용량
        public Bottle(ItemData itemdata_) : base(itemdata_) { }
        public override void Use(Player player)
        {
            // 용량이 있다면
            if (ownCapacity >= usePerValue)
            {
                player.Stat.OwnThirst += usePerValue;
                ownCapacity -= usePerValue;
            }
            else //용량이 없다면
            {
                if (!player.IsOnWater) return; //IsOnWater = 플레이어가 물 근처에 있을경우.
                ownCapacity = maxCapacity;
            }

        }
    }
    /*
     *                 if((item is Used_Item)) //소모 아이템이라면
                {
                    if (item.isInfinite) // 사용해도 사라지지않는 물병 같은 아이템이면,
                    {
                        Bottle used_Item = (Bottle)item;
                        used_Item.Use(owner);
                        Debug.Log("무한 내구도 소모 아이템 사용");
                    }
                    else //아닐 경우
                    {
                        curSlot.ItemCount--;
                        item.Use(owner);
                        Debug.Log("소모 아이템 사용");
                    }
                }
     * 
     * 
     *
     */ 
}
