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

        // 오브젝트 생성 순서 
        // 1. Buildable
        // 2. Water
        [SerializeField]
        private GroundSet groundSet;
        // 3. Obstacle
        [SerializeField]
        private ObstacleSpawner obstacleSpawner;
        // 4. Animal
        // 5. Monster
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        private void Awake()
        {
            monsterSpawner.OnAllMonsterDie += ExitNight;
            dayController.OnNight += EnterNight;
            // 그라운드 세팅이 끝난경우
            groundSet.OnEndGroundSet += ExitNight;
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
            obstacleSpawner.SpawnObstacle(day);
        }
    }
}
