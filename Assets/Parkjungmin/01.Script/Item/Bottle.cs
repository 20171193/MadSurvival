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
        [SerializeField] int maxCapacity; //�ִ� �뷮
        [SerializeField] public int ownCapacity; //���� �뷮
        [SerializeField] int usePerValue; //�ѹ� ���� �� ������ �ؼҵǴ� ��.

        public UnityAction OnUseBottle;

        public Bottle(ItemData itemdata_) : base(itemdata_) { }
        public override void Use(Player player)
        {
            // �뷮�� �ִٸ�
            if (ownCapacity >= usePerValue)
            {
                if (!player.IsOnWater)
                {

                    player.Stat.OwnThirst += usePerValue;
                    ownCapacity -= usePerValue;
                    OnUseBottle();
                    // ����õ ����
                    ScoreboardInvoker.Instance.drinkWater?.Invoke(ScoreType.Water);
                }
                else
                {
                    ownCapacity = maxCapacity;
                    OnUseBottle();
                }
            }
            else //�뷮�� ���ٸ�
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
