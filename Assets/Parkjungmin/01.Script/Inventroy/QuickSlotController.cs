using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jungmin
{
    public class QuickSlotController : MonoBehaviour
    {
        [SerializeField] public static QuickSlotController instance;

        [SerializeField] public Slot[] slots;
        [SerializeField] public GameObject Slot_parent;
        [SerializeField] public GameObject quickSlot_Base;


        private void Start()
        {
            instance = this;
            slots = Slot_parent.GetComponentsInChildren<Slot>();
        }

    }
}
