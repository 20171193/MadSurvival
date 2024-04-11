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
    [SerializeField] public Slot selectedSlot;
    [SerializeField] public Slot default_Slot; // 퀵슬롯의 초기 슬롯.맨 왼쪽 슬롯.
    public Slot SelectedSlot
    {
        get { return selectedSlot; }

        set
        {
            selectedSlot = value;
            player.ItemController.OnSelectQuickSlot(selectedSlot);//
        }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // 1. 게임 처음 시작 시 퀵슬롯의 기본 슬롯 지정.
        SelectedSlot = default_Slot;
        default_Slot.SetColorBG(0);
    }

}
