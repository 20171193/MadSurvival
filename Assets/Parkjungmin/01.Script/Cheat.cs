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
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["������ ����"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["������ ���"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["����"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["����"], 100);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["ö��"], 100);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["ưư�� ����"], 100);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["���̾Ƹ��"], 100);

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
                    //Debug.Log("���� ä����");
                }
            }
        }
        Destroy(gameObject);
    }

}
