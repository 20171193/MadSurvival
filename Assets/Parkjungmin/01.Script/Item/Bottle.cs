using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "Stamina_Potion", menuName = "Item/Used_Item/Potion/Stamina_Potion")]
    public class Bottle : Used_Item
    {
        [SerializeField] float maxCapacity; //�ִ� �뷮
        [SerializeField] float ownCapacity; //���� �뷮

        [SerializeField] float usePerValue; //�ѹ� ���ô� �뷮
        public Bottle(ItemData itemdata_) : base(itemdata_) { }
        public override void Use(Player player)
        {
            // �뷮�� �ִٸ�
            if (ownCapacity >= usePerValue)
            {
                player.Stat.OwnThirst += usePerValue;
                ownCapacity -= usePerValue;
            }
            else //�뷮�� ���ٸ�
            {
                if (!player.IsOnWater) return; //IsOnWater = �÷��̾ �� ��ó�� �������.
                ownCapacity = maxCapacity;
            }

        }
    }
    /*
     *                 if((item is Used_Item)) //�Ҹ� �������̶��
                {
                    if (item.isInfinite) // ����ص� ��������ʴ� ���� ���� �������̸�,
                    {
                        Bottle used_Item = (Bottle)item;
                        used_Item.Use(owner);
                        Debug.Log("���� ������ �Ҹ� ������ ���");
                    }
                    else //�ƴ� ���
                    {
                        curSlot.ItemCount--;
                        item.Use(owner);
                        Debug.Log("�Ҹ� ������ ���");
                    }
                }
     * 
     * 
     *
     */ 
}
