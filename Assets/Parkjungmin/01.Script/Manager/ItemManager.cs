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
    private void LoadItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Crafting:
                Item[] craft_items = Resources.LoadAll<Item>("CraftingItem"); //������ ��ųʸ��� ������ �Ҵ�
                foreach(Item item in craft_items) // 05.Scriptable Object���� �˻��ؼ� �Ҵ���
                { // ���������� �۵��ϱ� ���ؼ�, 
                    string name = item.itemdata.itemName;
                    //Debug.Log($"{name}");
                    craftingItemDic.Add(name, item);

                }
                break;
            case ItemType.Ingredient:
                Item[] igd_items = Resources.LoadAll<Item>("IngredientItem"); //������ ��ųʸ��� ������ �Ҵ�
                foreach (Item item in igd_items) // 05.Scriptable Object���� �˻��ؼ� �Ҵ���
                { // ���������� �۵��ϱ� ���ؼ�, 
                    string name = item.itemdata.itemName;
                    //Debug.Log($"{name}");
                    ingredientItemDic.Add(name, item);

                }
                break;
        }
    }
    void SearchForCraft()
    { //� ��� �������� �ִ��� ���� �������� üũ
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        ready_Craft = false;
        for (int x = 0; x < BackPackController.instance.slots.Length; x++) // ���� ������ ���� ��ŭ
        {
            
            Debug.Log($"ù��° ��� Ž��");
            if(BackPackController.instance.slots[x].item == null)
            {
                Debug.Log("�� ���Կ� �� �������� �����ϴ�.");
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
    public void CraftItem()
    {

        if (SelectedSlot_Recipe.instance.slot != null) //�������� ������ �������� ��
        {
            SearchForCraft();

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
