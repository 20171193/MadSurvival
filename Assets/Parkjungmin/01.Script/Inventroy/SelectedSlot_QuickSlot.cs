using Jc;
using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSlot_QuickSlot : MonoBehaviour //pointclick �ڵ鷯�� ���õ� ����(�κ��丮)
{
    public static SelectedSlot_QuickSlot instance;
    [SerializeField] Player player;
    [SerializeField] public Slot selectedSlot;
    [SerializeField] public Slot default_Slot; // �������� �ʱ� ����.�� ���� ����.
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
        // 1. ���� ó�� ���� �� �������� �⺻ ���� ����.
        SelectedSlot = default_Slot;
        default_Slot.SetColorBG(0);
    }

}
