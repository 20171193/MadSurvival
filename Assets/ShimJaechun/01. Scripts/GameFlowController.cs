using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [Space(2)]
        [SerializeField]
        private int maxDay;
        [SerializeField]
        private float dayChangeTime;

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

        [SerializeField]
        private DayAndNight dayController;

        [Space(3)]
        [Header("게임플레이 타이머 이벤트")]
        [Space(2)]
        public UnityEvent<int> OnCountTotalTime;
        public UnityEvent<int> OnCountDayTime;
        [Space(3)]
        [Header("낮/밤 변경 이벤트")]
        [Space(2)]
        public UnityEvent<int> OnEnterDay;
        public UnityEvent OnEnterNight;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private int day = 0;
        public int Day { get { return day; } set { day = value; } }

        [SerializeField]
        private float totalTime;
        public float TotalTime 
        { 
            get { return totalTime; } 
            set 
            {
                int prevTime = (int)totalTime;
                totalTime = value;
                OnCountTotalTime?.Invoke((int)value);
            }
        }

        [SerializeField]
        private float dayTime;
        public float DayTime 
        { 
            get { return dayTime; }
            set
            {
                int prevTime = (int)dayTime;
                dayTime = value;
                OnCountDayTime?.Invoke((int)value);
            }
        }

        private Coroutine totalTimer;

        private bool isNight;
        public bool IsNight { get { return isNight; }  set { isNight = value; } }

        private void Awake()
        {
            dayController.resetTimeValue = dayChangeTime;
            dayController.dayTimer = dayChangeTime;

            monsterSpawner.OnAllMonsterDie += ExitNight;
            dayController.OnNight += EnterNight;
            // 그라운드 세팅이 끝난경우
            groundSet.OnEndGroundSet += ExitNight;

            totalTimer = StartCoroutine(TotalGameTimer());
        }

        public void EnterNight()
        {
            isNight = true;
            OnEnterNight?.Invoke();
            // 동물 제거
            //낮-> 밤 변경
            // 몬스터 스폰
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            isNight = false;

            // 밤 -> 낮 변경
            dayController.OnExitNight();

            // 날짜 카운팅
            if (maxDay > day)
                day++;
            OnEnterDay?.Invoke(day);
            obstacleSpawner.SpawnObstacle(day);
        }

        IEnumerator TotalGameTimer()
        {
            while(true)
            {
                TotalTime += Time.deltaTime;
                if (!isNight)
                {
                    DayTime += Time.deltaTime;
                    if(DayTime >= dayChangeTime)
                    {
                        EnterNight();
                        dayTime = 0f;
                    }
                }
                yield return Time.deltaTime;
            }
        }
    }
}
