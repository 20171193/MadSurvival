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
        [Header("������ ����")]
        [Space(2)]
        [SerializeField]
        private int maxDay;
        [SerializeField]
        private float dayChangeTime;

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

        [SerializeField]
        private DayAndNight dayController;

        [Space(3)]
        [Header("�����÷��� Ÿ�̸� �̺�Ʈ")]
        [Space(2)]
        public UnityEvent<int> OnCountTotalTime;
        public UnityEvent<int> OnCountDayTime;
        [Space(3)]
        [Header("��/�� ���� �̺�Ʈ")]
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
            // �׶��� ������ �������
            groundSet.OnEndGroundSet += ExitNight;

            totalTimer = StartCoroutine(TotalGameTimer());
        }

        public void EnterNight()
        {
            isNight = true;
            OnEnterNight?.Invoke();
            // ���� ����
            //��-> �� ����
            // ���� ����
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            isNight = false;

            // �� -> �� ����
            dayController.OnExitNight();

            // ��¥ ī����
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
