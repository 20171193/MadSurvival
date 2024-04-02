using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField]
        private int day;
        public int Day { get { return day; } set { day = value; } }

        [SerializeField]
        private DayAndNight dayController;
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        public void EnterNight()
        {
            // 동물 제거

            // 몬스터 스폰
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            // 밤 -> 낮 변경
            dayController.IsNight = true;
            // 날짜 카운팅
            day++;
        }
    }
}
