using Jc;
using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSlot_QuickSlot : MonoBehaviour //pointclick 핸들러로 선택된 슬롯(인벤토리)
{
    public static SelectedSlot_QuickSlot instance;
    [SerializeField] Player player;
    [SerializeField] Slot selectedSlot;
    [SerializeField] Slot default_Slot;
    public Slot SelectedSlot
    {
        get { return selectedSlot; }

        set
        {
            selectedSlot = value;
            player.ItemController.OnSelectQuickSlot(selectedSlot);
        }
    }

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        SelectedSlot = default_Slot;
        default_Slot.SetColorBG(0);
    }

}
