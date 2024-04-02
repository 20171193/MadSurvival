using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField]
        private int day = 0;
        public int Day { get { return day; } set { day = value; } }

        [SerializeField]
        private DayAndNight dayController;
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        private void Awake()
        {
            dayController.OnNight += EnterNight;
        }

        private void OnEnable()
        {
            ExitNight();
        }

        public void EnterNight()
        {
            // 동물 제거
            //낮-> 밤 변경
            // 몬스터 스폰
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
			// 밤 -> 낮 변경
			dayController.OnExitNight();
			// 날짜 카운팅
			day++;
        }
    }
}
