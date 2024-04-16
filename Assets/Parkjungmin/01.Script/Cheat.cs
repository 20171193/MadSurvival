using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    bool isOnce = false;

    private void Start()
    {
        if (!isOnce)
        {
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["조잡한 도끼"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["조잡한 곡괭이"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["물병"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["장작"], 100);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["철괴"], 100);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["튼튼한 목재"], 100);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["다이아몬드"], 100);

            isOnce = true;

            for(int x=0; x<QuickSlotController.instance.slots.Length;x++)
            {
                if (QuickSlotController.instance.slots[x] == null)
                {
                    continue;
                }
                if (QuickSlotController.instance.slots[x] != null &&QuickSlotController.instance.slots[x].item is Bottle)
                {
                    Bottle bottle = (Bottle)QuickSlotController.instance.slots[x].item;

                    bottle.ownCapacity = bottle.maxCapacity;
                    QuickSlotController.instance.slots[x].UpdateSlotCount();
                    //Debug.Log("물병 채워짐");
                }
            }
        }
        Destroy(gameObject);
    }

}
