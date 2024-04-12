using jungmin;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipSlotController: MonoBehaviour
{
    public static EquipSlotController instance;

    [SerializeField] Slot equipSlot;
    public Slot EquipSlot
    {
        get
        {
            return instance.equipSlot;
        }
        set
        {
            if (value.item is Equip_Item)
            {
                Equip_Item equip_Item = (Equip_Item)(value.item);
                if (equip_Item.equipType is Equip_Item.EquipType.Armor)
                {
                    instance.equipSlot = value;
                }
            }        
        }
    }
    private void Start()
    {
        instance = this;
    }



}
