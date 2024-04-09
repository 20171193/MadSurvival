using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
namespace jungmin
{
    public class SelectedSlot_Inventory : MonoBehaviour
    {
        public static SelectedSlot_Inventory instance;
        [SerializeField] Player player;

        [SerializeField] Slot selectedSlot;
        public Slot SelectedSlot 
        {
            get { return selectedSlot; }

            set 
            { 
                selectedSlot = value;
                player.OnSelectInventorySlot(selectedSlot);
            } 
        }

        private void Start()
        {
            instance = this;
        }
    }
}
