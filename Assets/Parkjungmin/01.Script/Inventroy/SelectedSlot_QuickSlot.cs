using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSlot_QuickSlot : MonoBehaviour //pointclick 핸들러로 선택된 슬롯(인벤토리)
{
    public static SelectedSlot_QuickSlot instance;
    public Slot slot;

    private void Start()
    {
        instance = this;
    }

}
