using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            instance = this;
            reader = CSVReader.Read("CraftingItem/RecipeData");
            ReadRecipeCSV();
            recipe_slots = Slot_parent.GetComponentsInChildren<RecipeSlot>();
            PutRecipeInSlot();
        }

        void PutRecipeInSlot()
        { //������ ����Ʈ -> ������ ����
            for(int x = 0; x < recipeList.Count; x++)
            {
                recipe_slots[x].recipe = recipeList[x];
               
            }
        }
        void ReadRecipeCSV()
        {

            for(int x=0;x<3;x++) //ũ������ �������� �ִ� ������ 3����� �����ϰ�
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
                { //SelectedSlot_Recipe.instance.slot.recipe.IGD_2 == ������ �����ε� �纹�� �ʿ�.

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

