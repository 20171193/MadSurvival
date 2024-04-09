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
       * 게임 시작 시 레시피 리더가 csv 파일의 모든 레시피를 읽어오고 Recipe배열로 저장.
       * 레시피를 크래프팅의 레시피 데이터에 할당.
       * 레시피 슬롯을 누르면 인벤토리의 레시피 text에 보여주기.
       * 
       * 이 오브젝트는 무조건 씬에 있어야 레시피를 불러 올 수 있음.
       */
        public static RecipeController instance;
        List<Dictionary<string, object>> reader;
        public List<Recipe> recipeList = new List<Recipe>(); //레시피 리스트

        [SerializeField] RecipeSlot[] recipe_slots; //레시피 슬롯들
        [SerializeField] GameObject Slot_parent;
        public TMP_Text Recipe_Info;
        public int Page_index;
        public int Max_index;
        public int Min_index;
        /*
         *0~5 1페이지.
         *6~11 2페이지.
         *12~17 3페이지
         *18~23 4페이지
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
        { //레시피 리스트 -> 레시피 슬롯
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
            Debug.Log($"전체 레시피 갯수: {ItemManager.Instance.craftingItemDic.Count}");

            for(int x=0;x<ItemManager.Instance.craftingItemDic.Count; x++) //크래프팅 아이템의 최대 종류를 6개라고 가정하고
            {
                Recipe recipe = new Recipe();

                recipe.name = (string)reader[x]["result"]; //레시피 이름

                if (reader[x]["IGD_1"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_1"]))
                {
                    //1번 재료
                    recipe.IGD_1.IGD_Name = (string)reader[x]["IGD_1"];
                    recipe.IGD_1.IGD_Count = (int)reader[x]["IGD_1_Count"];
                }
                if (reader[x]["IGD_2"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_2"]))
                {
                    //2번 재료
                    recipe.IGD_2.IGD_Name = (string)reader[x]["IGD_2"];
                    recipe.IGD_2.IGD_Count = (int)reader[x]["IGD_2_Count"];
                }

                if (reader[x]["IGD_3"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_3"]))
                {
                    //3번 재료
                    recipe.IGD_3.IGD_Name = (string)reader[x]["IGD_3"];
                    recipe.IGD_3.IGD_Count = (int)reader[x]["IGD_3_Count"];
                }

                recipeList.Add(recipe); //설계도 배열에 넣기
                
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

