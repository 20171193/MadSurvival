using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private void OnEnable()
    {
        BackPackController.instance.AcquireItem(ItemManager.Instance.craftingItemDic["Potion"], 10);
        BackPackController.instance.AcquireItem(ItemManager.Instance.craftingItemDic["PickAxe"], 10);

    }
}