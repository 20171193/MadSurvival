using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private void OnEnable()
    {
        BackPackController.instance.AcquireItem(ItemManager.Instance.ItemDic["Potion"], 10);
        BackPackController.instance.AcquireItem(ItemManager.Instance.ItemDic["Knife"], 1);
        BackPackController.instance.AcquireItem(ItemManager.Instance.ItemDic["Meat"], 15);


    }
}
