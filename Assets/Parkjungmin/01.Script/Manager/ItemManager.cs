using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Events;
using JetBrains.Annotations;
namespace jungmin
{
    public enum ItemType
    {
        Crafting,
        Ingredient
    }
}
public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<string, Item> ItemDic = new Dictionary<string, Item>();
    bool ready_Craft;

    private void Start()
    {
        ItemDicInit();
    }

    //Method : ���� ���� �� ������ ��ųʸ� �ʱ�ȭ
    void ItemDicInit()
    {
        Item[] items = Resources.LoadAll<Item>("ItemDic");
        foreach (Item item in items)
        {
            string name = item.itemdata.itemName;
            Debug.Log($"{name}");
            if (ItemDic.ContainsKey(name)) { continue; } // ���� ��ųʸ��� �� �������� �̹� �����ϰ� �־��ٸ� ����.
            ItemDic.Add(name, item);

        }
    }

    // Method : ũ������ ���� �κ��丮�� ����� ��� ���� ���� Ȯ�� ****
    void SearchForCraft()
    {
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        ready_Craft = false;
        Slot[] invenslots = BackPackController.instance.slots;
        Slot[] quickslots = QuickSlotController.instance.slots;

        //1�� ��� ������ üũ
        for (int x = 0; x < quickslots.Length; x++)
        {

            if (quickslots[x].item == null)
            {
                continue;
            }
            if (quickslots[x].item != null)
            {
                if (quickslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name)
                {
                    if (quickslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                    {
                        IGD_1_Check = true;
                        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                        { //���� ��ᰡ �����ϸ�.
                            break;
                        }
                        else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == null && SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                        { //2,3�� ���ս��� ���� ���.
                            ready_Craft = true;
                        }
                        break;
                    }
                    else //��� ������ �����ϸ�.
                    {
                        IGD_1_Check = false;
                        break;
                    }
                }
            }

        }
        //1�� ��� �κ��丮 üũ
        if (!IGD_1_Check)
        {
            for (int x = 0; x < invenslots.Length; x++)
            {
                if (invenslots[x].item == null)
                {
                    continue;
                }

                if (invenslots[x].item != null)
                {
                    if (invenslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name)
                    {
                        if (invenslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                        {

                            IGD_1_Check = true;
                            if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                            { //���� ��ᰡ �����ϸ�.
                                break;
                            }
                            else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == null && SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                            { //2,3�� ���ս��� ���� ���
                                ready_Craft = true;
                            }
                            break;
                        }
                        else
                        {
                            IGD_1_Check = false;
                            break;
                        }
                    }
                }

            }
        }
        //2�� ��� ������ üũ
        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null && IGD_1_Check)
        {
            for (int x = 0; x < quickslots.Length; x++) // ���� ������ ���� ��ŭ
            {

                if (quickslots[x].item == null)
                {
                    continue;
                }
                if (quickslots[x].item != null)
                {
                    if (quickslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name)
                    {
                        if (quickslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
                        {

                            IGD_2_Check = true;
                            if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                            {
                                break;
                            }
                            else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                            {
                                ready_Craft = true;
                            }
                            break;
                        }
                        else
                        {
                            IGD_2_Check = false;
                            break;
                        }
                    }
                }

            }
            //2�� ��� �κ��丮 üũ.
            if(SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null && (IGD_1_Check && !IGD_2_Check))
            {
                for (int x = 0; x < invenslots.Length; x++) // ���� ������ ���� ��ŭ
                {

                    Debug.Log($"�ι�° ��� Ž��");
                    if (invenslots[x].item == null)
                    {
                        Debug.Log("�� ���Կ� �� �������� �����ϴ�.");
                        continue;
                    }

                    if (invenslots[x].item != null)
                    {
                        if (invenslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name)
                        {
                            if (invenslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
                            {
                                Debug.Log("��� �������� ����մϴ�.");
                                IGD_2_Check = true;
                                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                                {
                                    break;
                                }
                                else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                                { //���ս� ���� 2������ ���.
                                    ready_Craft = true;
                                }
                                break;
                            }
                            else
                            {
                                IGD_2_Check = false;
                                break;
                            }
                        }
                    }

                }
            }
        }

        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null && IGD_1_Check && IGD_2_Check)
        {
            for (int x = 0; x < quickslots.Length; x++) // ������ ������ ���� ��ŭ
            {

                if (quickslots[x].item == null)
                {
                    continue;
                }
                if (quickslots[x].item != null)
                {
                    if (quickslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name)
                    {
                        if (quickslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                        {
                            ready_Craft = true;
                            return;
                        }
                    }
                }

            }
            for (int x = 0; x < invenslots.Length; x++) // ���� ������ ���� ��ŭ
            {

                Debug.Log($"����° ��� Ž��");
                if (invenslots[x].item == null)
                {
                    Debug.Log("�� ���Կ� �� �������� �����ϴ�.");
                    continue;
                }
                if (invenslots[x].item != null)
                {
                    if (invenslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name)
                    {
                        if (invenslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                        {

                            ready_Craft = true;
                            return;
                        }
                    }
                }

            }
        }

        return;
    }

    // Method : ũ������ ��� ****
    public void CraftItem()
    {
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        bool IGD_3_Check = false;
        Slot[] invenslots = BackPackController.instance.slots;
        Slot[] quickslots = QuickSlotController.instance.slots;


        if (SelectedSlot_Recipe.instance.slot != null)
        {
            SearchForCraft(); // 1. ���� ��ᰡ �κ��丮 ���� ����� �ִ��� Ȯ���Ѵ�.

            if (ready_Craft) // 2. ��ᰡ �غ� �� ���
            {
                //2. ���չ��� ��� ������ �κ��丮�� �߰�.
                BackPackController.instance.AcquireItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe_name]);
                BackPackController.instance.craftsound?.Play();
                Debug.Log("ũ������ ����");


                //2.2 ��� �������� ������ �Ҹ�.

                for (int x = 0; x < quickslots.Length; x++) //�������� ������ �Ҹ�.
                {
                    if (quickslots[x].item != null && (SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name == quickslots[x].item.itemdata.itemName))
                    {
                        if (QuickSlotController.instance.slots[x].itemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                        {
                            QuickSlotController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);
                            IGD_1_Check = true;
                        }
                        break;
                    }
                }
                if(!IGD_1_Check)
                {
                    BackPackController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);
                }



                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                {
                    for (int x = 0; x < quickslots.Length; x++) //�������� ������ �Ҹ�.
                    {
                        if (quickslots[x].item != null && (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == quickslots[x].item.itemdata.itemName))
                        {
                            if (QuickSlotController.instance.slots[x].itemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
                            {
                                QuickSlotController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                                IGD_2_Check = true;
                            }
                            break;
                        }
                    }
                    if (!IGD_2_Check)
                    {
                        BackPackController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                    }

                    if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                    {
                        for (int x = 0; x < quickslots.Length; x++) //�������� ������ �Ҹ�.
                        {
                            if (quickslots[x].item != null && (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == quickslots[x].item.itemdata.itemName))
                            {
                                if (QuickSlotController.instance.slots[x].itemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                                {
                                    QuickSlotController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                                    IGD_3_Check = true;
                                }
                                break;
                            }
                        }
                        if (!IGD_3_Check)
                        {
                            BackPackController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                        }
                   }

                }

            }
            else if(!ready_Craft)
            {
                    Debug.Log("��� ����");
            }
        }
    }
}
