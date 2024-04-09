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
        public TMP_Text Recipe_Info;
        public int Page_index;
        public int Max_index;
        public int Min_index;
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
        }

        void PutRecipeInSlot()
        { //������ ����Ʈ -> ������ ����
            for(int x = 0; x < 6; x++)
            {
               recipe_slots[x].recipe = recipeList[x + (6 * Page_index)];
            }
        }

        public void NextRecipePage()
        {
            if(Page_index < Max_index)
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

        void ReadRecipeCSV()
        {
            Debug.Log($"��ü ������ ����: {ItemManager.Instance.craftingItemDic.Count}");

            for(int x=0;x<ItemManager.Instance.craftingItemDic.Count; x++) //ũ������ �������� �ִ� ������ 6����� �����ϰ�
            {
                Recipe recipe = new Recipe();

                recipe.name = (string)reader[x]["result"]; //������ �̸�

                if (reader[x]["IGD_1"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_1"]))
                {
                    //1�� ���
                    recipe.IGD_1.IGD_Name = (string)reader[x]["IGD_1"];
                    recipe.IGD_1.IGD_Count = (int)reader[x]["IGD_1_Count"];
                }
                if (reader[x]["IGD_2"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_2"]))
                {
                    //2�� ���
                    recipe.IGD_2.IGD_Name = (string)reader[x]["IGD_2"];
                    recipe.IGD_2.IGD_Count = (int)reader[x]["IGD_2_Count"];
                }

                if (reader[x]["IGD_3"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_3"]))
                {
                    //3�� ���
                    recipe.IGD_3.IGD_Name = (string)reader[x]["IGD_3"];
                    recipe.IGD_3.IGD_Count = (int)reader[x]["IGD_3_Count"];
                }

                recipeList.Add(recipe); //���赵 �迭�� �ֱ�
                
            }
        }


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

