using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    bool isOnce = false;

    private void OnEnable()
    {
        if (!isOnce)
        {
            BackPackController.instance.AcquireItem(ItemManager.Instance.ItemDic["Bottle"], 1);
            BackPackController.instance.AcquireItem(ItemManager.Instance.ItemDic["PickAxe"], 1);
            BackPackController.instance.AcquireItem(ItemManager.Instance.ItemDic["Axe"], 1);
            isOnce = true;
        }
    }
}
