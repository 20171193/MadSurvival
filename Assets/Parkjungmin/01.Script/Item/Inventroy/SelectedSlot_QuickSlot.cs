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
    public Slot SelectedSlot
    {
        get { return selectedSlot; }

        set
        {
            selectedSlot = value;
            player.OnSelectSlot(selectedSlot);
        }
    }

    private void Start()
    {
        instance = this;
    }    

}
