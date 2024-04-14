using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace jungmin
{
    public class RecipeController : MonoBehaviour
    { /*
       * ���� ���� �� ������ ������ csv ������ ��� �����Ǹ� �о���� Recipe�迭�� ����.
       * �����Ǹ� ũ�������� ������ �����Ϳ� �Ҵ�.
       * ������ ������ ������ �κ��丮�� ������ text�� �����ֱ�.
       * 
       * �� ������Ʈ�� ������ ���� �־�� �����Ǹ� �ҷ� �� �� ����.
       */
        public static RecipeController instance;
        List<Dictionary<string, object>> reader;
        public List<Recipe> recipeList = new List<Recipe>(); //������ ����Ʈ

        [SerializeField] RecipeSlot[] recipe_slots; //������ ���Ե�
        [SerializeField] GameObject Slot_parent;
        [SerializeField]
        public TMP_Text Recipe_Info;
        public int Page_index;
        int Max_index;
        int Min_index;

        /*
         *0~5 1������.
         *6~11 2������.
         *12~17 3������
         *18~23 4������
         * 
         * 
         */

        private void Start()
        {
            instance = this;
            reader = CSVReader.Read("RecipeData/RecipeCSV");
            ReadRecipeCSV();
            recipe_slots = Slot_parent.GetComponentsInChildren<RecipeSlot>();
            Page_index = 0;
            PutRecipeInSlot();
            Max_index = (recipeList.Count % 6 == 0) ? recipeList.Count/6 : (recipeList.Count+5)/6;
        } // ��� 24 -> recipeList.Count = 22 ,recipeList.Count%6 = 4

        // Method : ������ ����Ʈ�� �����͸� ũ������ �������� �Ҵ� ****
        void PutRecipeInSlot()
        {
            for(int x = 0; x < recipe_slots.Length; x++)
            {  //1.���� ������ ����Ʈ�� ī���͸� �Ѿ�� ��� ���� ������ �ֱ�.
                if (x + (6 * Page_index) < recipeList.Count)
                {
                    recipe_slots[x].recipe = recipeList[x + (6 * Page_index)];
                }
                else if(x + (6 * Page_index) >= recipeList.Count)
                {
                    recipe_slots[x].recipe = recipeList[0];
                }
            }
        }
        // Method : ũ������ ������ ���� �������� �̵� ****
        public void NextRecipePage()
        {
            Debug.Log("Next");
            if (Page_index < Max_index)
            {
                Debug.Log($"{Page_index}");
                Page_index++;
                PutRecipeInSlot();
                foreach (RecipeSlot slot in recipe_slots)
                {
                    slot.ResetSlot();
                }
            }
        }
        // Method : ũ������ ������ ���� �������� �̵� ****
        public void PastRecipePage()
        {
            Debug.Log("Past");
            if (Page_index > Min_index)
            {
                Debug.Log($"{Page_index}");
                Page_index--;
                PutRecipeInSlot();
                foreach (RecipeSlot slot in recipe_slots)
                {
                    slot.ResetSlot();
                }
            }
        }
        // Method : ������ CSV ������ �����͸� ������ List�� ���� ****
        void ReadRecipeCSV()
        {
            // 1. ũ������ Dic�� ������ŭ, CSV ���Ͽ��� ũ������ ������ ������ �о�´�. 
            for(int x=0;x<ItemManager.Instance.ItemDic.Count; x++)
            {
                Recipe recipe = new Recipe();
                recipe.name = (string)reader[x]["result"];

                // 2. Ư�� �������� ���չ��� ù��° ����� �̸��� ������ �����Ѵ�. 
                if (reader[x]["IGD_1"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_1"]))
                {
                    recipe.IGD_1.IGD_Name = (string)reader[x]["IGD_1"];
                    recipe.IGD_1.IGD_Count = (int)reader[x]["IGD_1_Count"];
                }
                else
                {
                    return;
                }
                // 2.1 ���⼭���� ���ս��� ���� 2������ 3������ üũ�Ѵ�.

                // 2.2 Ư�� �������� ���չ��� �ι�° ����� �̸��� ������ �����Ѵ�.
                if (reader[x]["IGD_2"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_2"]))
                {
                    //2�� ���
                    recipe.IGD_2.IGD_Name = (string)reader[x]["IGD_2"];
                    recipe.IGD_2.IGD_Count = (int)reader[x]["IGD_2_Count"];
                }
                // 2.3 Ư�� �������� ���չ��� ����° ����� �̸��� ������ �����Ѵ�.
                if (reader[x]["IGD_3"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_3"]))
                {
                    recipe.IGD_3.IGD_Name = (string)reader[x]["IGD_3"];
                    recipe.IGD_3.IGD_Count = (int)reader[x]["IGD_3_Count"];
                }
                // 3. �ϼ��� ������ ��ü�� ������ ����Ʈ�� �߰��Ѵ�.
                recipeList.Add(recipe); 
                
            }
        }

        // Method : ���� �������� ���չ��� ũ������ ���� UI���� ��� ****
        public void ShowRecipeInfo()
        {
            if (SelectedSlot_Recipe.instance.slot != null)
            {
                StringBuilder text = new StringBuilder($"{SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name}*{SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count}");
                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                {

                    text.Append($"+{SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name}*{SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count}");
                }
                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                {
                    text.Append($"+{SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name}*{SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count}");
                }

                Debug.Log(text);

                Recipe_Info.text = text.ToString();
            }
        

        }
    }

}

