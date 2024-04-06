using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSlot_Inventory : MonoBehaviour
{
    public static SelectedSlot_Inventory instance;
    public Slot slot;


    private void Start()
    {
        instance = this;

    }


}
