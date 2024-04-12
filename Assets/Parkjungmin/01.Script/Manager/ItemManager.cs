using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Linq;
using Unity.VisualScripting;
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
 
    public Dictionary<string, Item> craftingItemDic = new Dictionary<string, Item>();
    public Dictionary<string, Item> ingredientItemDic = new Dictionary<string, Item>();
    bool ready_Craft;
    private void Start()
    {
        LoadItem(ItemType.Ingredient);
        LoadItem(ItemType.Crafting);
    }

    // Method : �������� Ÿ�Կ� ���� ������ ������ Dic�� �߰� ****
    private void LoadItem(ItemType type)
    {
        // 1.Resource�� CraftingItem,IngredientItem �������� ��� Item �Ӽ��� ���� �ִ� �����յ��� �����Ѵ�.
        switch (type)
        {
            case ItemType.Crafting: // 2. ũ������ �Ӽ��̶�� ũ������ Dic�� ������ �߰�
                Item[] craft_items = Resources.LoadAll<Item>("CraftingItem"); 
                foreach(Item item in craft_items)
                {
                    string name = item.itemdata.itemName;
                    //Debug.Log($"Craft: {name}");
                    if (craftingItemDic.ContainsKey(name)) { continue; }
                    craftingItemDic.Add(name, item);

                }
                break;
            case ItemType.Ingredient: // 2. ��� �Ӽ��̶�� ũ������ Dic�� ������ �߰�
                Item[] igd_items = Resources.LoadAll<Item>("IngredientItem");
                foreach (Item item in igd_items)
                { 
                    string name = item.itemdata.itemName;
                    //Debug.Log($"IGD : {name}");
                    if (ingredientItemDic.ContainsKey(name)) { continue; }
                    ingredientItemDic.Add(name, item);

                }
                break;
        }
    }


    // Method : ũ������ ���� �κ��丮�� ����� ��� ���� ���� Ȯ�� ****
    void SearchForCraft()
    {
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        ready_Craft = false;
        for (int x = 0; x < BackPackController.instance.slots.Length; x++) // ���� ������ ���� ��ŭ
        {
            
            if(BackPackController.instance.slots[x].item == null)
            {
                continue;
            }
            if (BackPackController.instance.slots[x].item != null)
            {
                if (BackPackController.instance.slots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name)
                {
                    if (BackPackController.instance.slots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                    {

                        IGD_1_Check = true;
                        if(SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                        {
                            break;
                        }
                        else if(SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == null && SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null )
                        { //���ս� ���� 1���ۿ� ���� ���.
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

        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null && IGD_1_Check)
        {
            for (int x = 0; x < BackPackController.instance.slots.Length; x++) // ���� ������ ���� ��ŭ
            {

                Debug.Log($"�ι�° ��� Ž��");
                if (BackPackController.instance.slots[x].item == null)
                {
                    Debug.Log("�� ���Կ� �� �������� �����ϴ�.");
                    continue;
                }

                if (BackPackController.instance.slots[x].item != null)
                {
                    if (BackPackController.instance.slots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name)
                    {
                        if (BackPackController.instance.slots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
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
        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null && IGD_1_Check && IGD_2_Check)
        {
            for (int x = 0; x < BackPackController.instance.slots.Length; x++) // ���� ������ ���� ��ŭ
            {

                Debug.Log($"����° ��� Ž��");
                if (BackPackController.instance.slots[x].item == null)
                {
                    Debug.Log("�� ���Կ� �� �������� �����ϴ�.");
                    continue;
                }
                if (BackPackController.instance.slots[x].item != null)
                {
                    if (BackPackController.instance.slots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name)
                    {
                        if (BackPackController.instance.slots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                        {

                            ready_Craft = true;
                            break;
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

        if (SelectedSlot_Recipe.instance.slot != null) 
        {
            SearchForCraft(); // 1. ���� ��ᰡ �κ��丮 ���� ����� �ִ��� Ȯ���Ѵ�.

            if (ready_Craft)
            {
                BackPackController.instance.AcquireItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe_name]);
                Debug.Log("ũ������ ����");

                if(craftingItemDic.ContainsKey(SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name))
                    BackPackController.instance.LoseItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);
                else
                    BackPackController.instance.LoseItem(ingredientItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);

                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                {
                    if (craftingItemDic.ContainsKey(SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name))
                        BackPackController.instance.LoseItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                    else
                        BackPackController.instance.LoseItem(ingredientItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                }
                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                {
                    if (craftingItemDic.ContainsKey(SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name))
                        BackPackController.instance.LoseItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                    else
                        BackPackController.instance.LoseItem(ingredientItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                }

                //�������� ������ ���

                //���� �Ҹ��� �������� ���� ���̴� ��� �߰�
            }
            else
            {
                Debug.Log("��� ����");
            }

        }
    }
}
