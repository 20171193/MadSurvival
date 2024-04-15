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
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["Axe"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["PickAxe"], 1);
            QuickSlotController.instance.AcquireItem(ItemManager.Instance.ItemDic["Bottle"], 1);
            isOnce = true;
        }
        Destroy(gameObject);
    }

}
