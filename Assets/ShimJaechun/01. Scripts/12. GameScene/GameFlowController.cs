using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using System;
using Unity.VisualScripting;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        public static GameFlowController inst;
        public static GameFlowController Inst { get { return inst; } }   

        [Header("������ ����")]
        [Space(2)]
        [SerializeField]
        private int maxDay;
        [SerializeField]
        private float dayChangeTime;
        public float DayChangeTime { get { return dayChangeTime; } }
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
        private Player player;

        [Header("�� / �� ���� �ý���")]
        [SerializeField]
        private DayAndNight dayController;
        private Vector3 dayRot = new Vector3(70f, -30f, 0);     // ������ �¾� ȸ����
        private Vector3 nightRot = new Vector3(270f, -30f, 0);  // ������ �¾� ȸ����
        private float nightFogDensity = 0.18f;
        private float dayFogDensity = 0f;

        [SerializeField]
        private Light playerSpotLight;

        [Header("���̵� ��/�ƿ�")]
        [SerializeField]
        private Image fadeImage;
        [SerializeField]
        private GameObject fadeCanvas;
        [SerializeField]
        private GameObject lootButtonGroup;
        [SerializeField]
        private float fadeTime;
        private Color fadeOutColor = new Color(0, 0, 0, 0);
        private Color fadeInColor = new Color(0, 0, 0, 1);

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
        private Coroutine fadeRoutine;

        private bool isNight;
        public bool IsNight { get { return isNight; }  set { isNight = value; } }

        private void Awake()
        {
            inst = this;

            dayController.resetTimeValue = dayChangeTime;
            dayController.dayTimer = dayChangeTime;

            dayController.OnNight += EnterNight;

            totalTimer = StartCoroutine(TotalGameTimer());
        }

        public void EnterNight()
        {
            // �¾� ȸ���� ����
            dayController.transform.eulerAngles = nightRot;
            // �Ȱ� �е� ����
            RenderSettings.fogDensity = nightFogDensity;
            // �÷��̾� ����Ʈ����Ʈ ����
            playerSpotLight.intensity = 20f;

            isNight = true;
            OnEnterNight?.Invoke();
            // ���� ����
            //��-> �� ����
            // ���� ����
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            // �¾� ȸ���� ����
            dayController.transform.eulerAngles = dayRot;
            // �Ȱ� �е� ����
            RenderSettings.fogDensity = nightFogDensity;
            // �÷��̾� ����Ʈ����Ʈ ����
            playerSpotLight.intensity = 0f;

            isNight = false;

            // �� -> �� ����
            dayController.OnExitNight();

            // ��¥ ī����
            if (maxDay > day)
                day++;

            OnEnterDay?.Invoke(day);
            obstacleSpawner.SpawnObstacle(day);
            SetPlayerPosition();
        }

        IEnumerator TotalGameTimer()
        {
            float dayRate = 0f;
            while(true)
            {
                TotalTime += Time.deltaTime;
                if (!isNight)
                {
                    DayTime += Time.deltaTime;
                    dayRate = DayTime / DayChangeTime;

                    dayController.transform.eulerAngles = Vector3.Lerp(dayRot, nightRot, dayRate);      // �¾� ȸ�� �� ����
                    RenderSettings.fogDensity = Mathf.Lerp(dayFogDensity, nightFogDensity, dayRate);    // �Ȱ� �е� �� ����
                    playerSpotLight.intensity = Mathf.Lerp(0, 10f, dayRate-0.05f);        // �÷��̾� ����Ʈ����Ʈ ��� ����
                    if(DayTime >= dayChangeTime)
                    {
                        EnterNight();
                        dayTime = 0f;
                    }
                }
                yield return null;
            }
        }

        public void FadeControll(bool isFadeIn)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(isFadeIn ? FadeInRoutine() : FadeOutRoutine());
        }
        // ���̵���
        IEnumerator FadeInRoutine()
        {
            Time.timeScale = 0f;
            yield return null;

            float rate = 0f;
            while(rate <= 1)
            {
                rate += Time.unscaledDeltaTime / fadeTime;
                fadeImage.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
                yield return null;
            }

            lootButtonGroup.SetActive(true);
            fadeImage.color = fadeInColor;
            yield return null;
        }
        // ���̵�ƿ�
        IEnumerator FadeOutRoutine()
        {
            ExitNight();
            lootButtonGroup.SetActive(false);
            yield return null;

            float rate = 0f;
            while (rate <= 1)
            {
                rate += Time.unscaledDeltaTime / fadeTime;
                fadeImage.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
                yield return null;
            }

            fadeImage.color = fadeOutColor;
            Time.timeScale = 1f;
            yield return null;
        }

        private void SetPlayerPosition()
        {
            player.transform.position =
                Manager.Navi.gameMap[Manager.Navi.cornerTL.z + (Manager.Navi.cornerBL.z - Manager.Navi.cornerTL.z) / 2].
                groundList[Manager.Navi.cornerTL.x + (Manager.Navi.cornerTR.x - Manager.Navi.cornerTL.x) / 2].transform.position;
            player.transform.Translate(Vector3.up * 1f);
        }
    }
}
