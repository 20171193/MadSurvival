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
        public void LoseItem(Item _item, int _count = 1) //�������� �Ұ���. 
        {
            if (_item.itemdata.itemtype != ItemData.ItemType.Equipment)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // ���� ������ �ִ� ������ �߰����� ��.
                    {
                        if (slots[i].item.itemdata.itemName == _item.itemdata.itemName) // �ش� �������� ã�� ������ ���ҽ�Ų��.
                        {
                            slots[i].SetSlotCount(-_count);
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++) // ȹ�� ������ �Ӽ��� ����
            {
                if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // �� ������ ã�� �׳� �ִ´�.
                {
                    slots[i].ClearSlot();
                    return;
                }
            }
        }
        public void AcquireItem(Item _item, int _count = 1) //�������� �Ծ��� �� �κ��丮�� �ִ� ���,�ڵ����� ���ĵǾ� �߰���
        {
            Slot[] quickslots = QuickSlotController.instance.slots;

            if (_item.itemdata.itemtype != ItemData.ItemType.Equipment) //ȹ�� ������ �Ӽ��� ��� �ƴϸ�
            {

                for (int i = 0; i < quickslots.Length; i++) //���� �������� ���鼭
                {
                    if (quickslots[i].item != null && (quickslots[i].item.itemdata.itemName == _item.itemdata.itemName)) // ���� ������ �ִ� ������ �߰����� ��.
                    {
                        quickslots[i].SetSlotCount(_count);
                        quickslots[i].UpdateSlotCount();
                        return;
                    }
                }
                for (int i = 0; i < quickslots.Length; i++) //���� �������� ������ ���.
                {

                    if (quickslots[i].item == null)
                    {
                        quickslots[i].AddItem(_item, _count);
                        return;
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++) // ȹ�� ������ �Ӽ��� ����
            {
                if (quickslots[i].item == null) // �� ������ ã�� �׳� �ִ´�.
                {
                    quickslots[i].AddItem(_item, _count);
                    return;
                }
            }
        }
    }
}
