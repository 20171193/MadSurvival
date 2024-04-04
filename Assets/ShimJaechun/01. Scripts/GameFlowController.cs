using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Runtime.CompilerServices;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField]
        private int maxDay;

        [SerializeField]
        private int day = 0;
        public int Day { get { return day; } set { day = value; } }

        [SerializeField]
        private DayAndNight dayController;

        // ������Ʈ ���� ���� 
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
            // �׶��� ������ �������
            groundSet.OnEndGroundSet += ExitNight;
        }

        public void EnterNight()
        {
            // ���� ����
            //��-> �� ����
            // ���� ����
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            // �� -> �� ����
            dayController.OnExitNight();

            // ��¥ ī����
            if (maxDay > day)
                day++;

            obstacleSpawner.SpawnObstacle(day);
        }
    }
}
