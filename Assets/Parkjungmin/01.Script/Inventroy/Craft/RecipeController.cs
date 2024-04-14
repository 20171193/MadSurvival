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
        [SerializeField]
        public TMP_Text Recipe_Info;
        public int Page_index;
        int Max_index;
        int Min_index;

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
            Max_index = (recipeList.Count % 6 == 0) ? recipeList.Count/6 : (recipeList.Count+5)/6;
        } // 기댓값 24 -> recipeList.Count = 22 ,recipeList.Count%6 = 4

        // Method : 레시피 리스트의 데이터를 크래프팅 슬롯으로 할당 ****
        void PutRecipeInSlot()
        {
            for(int x = 0; x < recipe_slots.Length; x++)
            {  //1.만일 레시피 리스트의 카운터를 넘어서는 경우 오류 레시피 넣기.
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
        // Method : 크래프팅 슬롯의 다음 페이지로 이동 ****
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
        // Method : 크래프팅 슬롯의 이전 페이지로 이동 ****
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
        // Method : 레시피 CSV 파일의 데이터를 레시피 List에 넣음 ****
        void ReadRecipeCSV()
        {
            // 1. 크래프팅 Dic의 개수만큼, CSV 파일에서 크래프팅 제조법 갯수를 읽어온다. 
            for(int x=0;x<ItemManager.Instance.ItemDic.Count; x++)
            {
                Recipe recipe = new Recipe();
                recipe.name = (string)reader[x]["result"];

                // 2. 특정 아이템의 조합법의 첫번째 재료의 이름과 갯수를 저장한다. 
                if (reader[x]["IGD_1"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_1"]))
                {
                    recipe.IGD_1.IGD_Name = (string)reader[x]["IGD_1"];
                    recipe.IGD_1.IGD_Count = (int)reader[x]["IGD_1_Count"];
                }
                else
                {
                    return;
                }
                // 2.1 여기서부터 조합식의 항이 2개인지 3개인지 체크한다.

                // 2.2 특정 아이템의 조합법의 두번째 재료의 이름과 갯수를 저장한다.
                if (reader[x]["IGD_2"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_2"]))
                {
                    //2번 재료
                    recipe.IGD_2.IGD_Name = (string)reader[x]["IGD_2"];
                    recipe.IGD_2.IGD_Count = (int)reader[x]["IGD_2_Count"];
                }
                // 2.3 특정 아이템의 조합법의 세번째 재료의 이름과 갯수를 저장한다.
                if (reader[x]["IGD_3"] != null && !string.IsNullOrEmpty((string)reader[x]["IGD_3"]))
                {
                    recipe.IGD_3.IGD_Name = (string)reader[x]["IGD_3"];
                    recipe.IGD_3.IGD_Count = (int)reader[x]["IGD_3_Count"];
                }
                // 3. 완성된 레시피 객체를 레시피 리스트에 추가한다.
                recipeList.Add(recipe); 
                
            }
        }

        // Method : 조합 아이템의 조합법을 크래프팅 슬롯 UI에서 띄움 ****
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

