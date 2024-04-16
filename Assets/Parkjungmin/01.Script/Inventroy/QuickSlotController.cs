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


        private void Awake()
        {
            instance = this;
            slots = Slot_parent.GetComponentsInChildren<Slot>();
        }
        public void LoseItem(Item _item, int _count = 1) //아이템을 잃게함. 
        {
            if (_item.itemdata.itemtype != ItemData.ItemType.Equipment)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // 같은 아이템 있는 슬롯을 발견했을 때.
                    {
                        if (slots[i].item.itemdata.itemName == _item.itemdata.itemName) // 해당 아이템을 찾아 개수를 감소시킨다.
                        {
                            slots[i].SetSlotCount(-_count);
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++) // 획득 아이템 속성이 장비면
            {
                if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // 빈 슬롯을 찾아 그냥 넣는다.
                {
                    slots[i].ClearSlot();
                    return;
                }
            }
        }
        public void AcquireItem(Item _item, int _count = 1) //아이템을 먹었을 때 인벤토리에 넣는 기능,자동으로 정렬되어 추가됨
        {
            Slot[] quickslots = QuickSlotController.instance.slots;

            if (_item.itemdata.itemtype != ItemData.ItemType.Equipment) //획득 아이템 속성이 장비가 아니면
            {

                for (int i = 0; i < quickslots.Length; i++) //먼저 퀵슬롯을 돌면서
                {
                    if (quickslots[i].item != null && (quickslots[i].item.itemdata.itemName == _item.itemdata.itemName)) // 같은 아이템 있는 슬롯을 발견했을 때.
                    {
                        quickslots[i].SetSlotCount(_count);
                        quickslots[i].UpdateSlotCount();
                        return;
                    }
                }
                for (int i = 0; i < quickslots.Length; i++) //같은 아이템이 없었을 경우.
                {

                    if (quickslots[i].item == null)
                    {
                        quickslots[i].AddItem(_item, _count);
                        return;
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++) // 획득 아이템 속성이 장비면
            {
                if (quickslots[i].item == null) // 빈 슬롯을 찾아 그냥 넣는다.
                {
                    quickslots[i].AddItem(_item, _count);
                    return;
                }
            }
        }
    }
}
