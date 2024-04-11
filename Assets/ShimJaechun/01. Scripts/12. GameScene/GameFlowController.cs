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

        [Header("에디터 세팅")]
        [Space(2)]
        [SerializeField]
        private GameObject dayLight;
        [SerializeField]
        private GameObject nightLight;

        [SerializeField]
        private int maxDay;
        [SerializeField]
        private float dayChangeTime;
        public float DayChangeTime { get { return dayChangeTime; } }
        // 오브젝트 생성 순서 
        // 1. Buildable
        // 2. Water
        [SerializeField]
        private GroundSet groundSet;
        // 3. Obstacle
        [SerializeField]
        private ObstacleSpawner obstacleSpawner;
        // 4. Animal
        [SerializeField]
        private AnimalSpawner animalSpawner;
        // 5. Monster
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        [SerializeField]
        private Player player;

        [Header("낮 / 밤 변경 시스템")]
        [SerializeField]
        private DayAndNight dayController;
        [SerializeField]
        private float fogSpeed;
        private Vector3 dayRot = new Vector3(70f, 0, 0);     // 정오의 태양 회전값
        private Vector3 nightRot = new Vector3(270f, 0, 0);  // 자정의 태양 회전값
        private Vector3 moonDayRot = new Vector3(270, 0, 0);   // 정오의 달 회전값
        private Vector3 moonNightRot = new Vector3(470, 0, 0);   // 자정의 달 회전값

        private float nightFogDensity = 0.18f;
        private float dayFogDensity = 0f;

        [SerializeField]
        private Light playerSpotLight;

        [Header("페이드 인/아웃")]
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
        private Coroutine fadeRoutine;
        private Coroutine enterFogRoutine;

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
            // 동물 리턴
            animalSpawner.ReturnAllAnimal();

            // 태양 회전값 적용
            dayLight.transform.eulerAngles = nightRot;
            // 달 회전값 적용
            nightLight.transform.eulerAngles = moonNightRot;
            // 안개 밀도 적용
            RenderSettings.fog = true;
            // 플레이어 스포트라이트 적용
            playerSpotLight.intensity = 10f;

            isNight = true;
            OnEnterNight?.Invoke();
            // 동물 제거
            //낮-> 밤 변경
            // 몬스터 스폰
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            // 태양 회전값 적용
            dayLight.transform.eulerAngles = dayRot;
            // 달 회전값 적용
            nightLight.transform.eulerAngles = moonDayRot;

            // 안개 밀도 적용
            RenderSettings.fog = false;
            // 플레이어 스포트라이트 적용
            playerSpotLight.intensity = 0f;

            isNight = false;

            // 밤 -> 낮 변경
            dayController.OnExitNight();

            // 날짜 카운팅
            if (maxDay > day)
                day++;

            OnEnterDay?.Invoke(day);
            obstacleSpawner.SpawnObstacle(day);
            animalSpawner.OnSpawn(day);
            SetPlayerPosition();
        }

        IEnumerator TotalGameTimer()
        {
            float dayRate = 0f;
            bool isEnterFog = false;

            while(true)
            {
                TotalTime += Time.deltaTime;
                if (!isNight)
                {
                    DayTime += Time.deltaTime;
                    dayRate = DayTime / DayChangeTime;

                    dayLight.transform.eulerAngles = Vector3.Lerp(dayRot, nightRot, dayRate);      // 태양 회전 값 적용
                    nightLight.transform.eulerAngles = Vector3.Lerp(moonDayRot, moonNightRot, dayRate);     // 달 회전 값 적용
                    //if (dayRate >= 0.9f && !isEnterFog)    // 안개 밀도 적용
                    //{
                    //    enterFogRoutine = StartCoroutine(EnterFogRoutine());
                    //    isEnterFog = true;
                    //}
                    playerSpotLight.intensity = Mathf.Lerp(0, 10f, dayRate-0.05f);        // 플레이어 스포트라이트 밝기 적용

                    if(DayTime >= dayChangeTime)
                    {
                        EnterNight();
                        DayTime = 0f;
                        //isEnterFog = false;
                    }
                }
                yield return null;
            }
        }
        IEnumerator EnterFogRoutine()
        {
            float rate = 0f;
            while(rate < 1f)
            {
                rate += Time.deltaTime * fogSpeed;
                RenderSettings.fogDensity = Mathf.Lerp(dayFogDensity, nightFogDensity, rate);
                yield return null;
            }
        }

        public void FadeControll(bool isFadeIn)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(isFadeIn ? FadeInRoutine() : FadeOutRoutine());
        }
        // 페이드인
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
        // 페이드아웃
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
