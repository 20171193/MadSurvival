using System.Collections;
using System.Collections.Generic;
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

        public Dictionary<string, Item> craftingItemDic;
        List<Dictionary<string, object>> reader;
        public List<Recipe> recipeList = new List<Recipe>(); //레시피 리스트

        [SerializeField] RecipeSlot[] recipe_slots; //레시피 슬롯들
        [SerializeField] GameObject Slot_parent;
        public TMP_Text Recipe_Info;

        private void Awake()
        {
            
        }

        private void LoadItem()
        {

        }

        private void Start()
        {
            reader = CSVReader.Read("CraftingItem/RecipeData");
            ReadRecipeCSV();
            recipe_slots = Slot_parent.GetComponentsInChildren<RecipeSlot>();

            PutRecipeInSlot();
        }

        void PutRecipeInSlot()
        { //레시피 리스트 -> 레시피 슬롯
            for(int x = 0; x < recipeList.Count; x++)
            {
                recipe_slots[x].recipe = recipeList[x];
            }
        }
        void ReadRecipeCSV()
        {

            for(int x=0;x<3;x++) //크래프팅 아이템의 최대 종류를 3개라고 가정하고
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


    }
}

