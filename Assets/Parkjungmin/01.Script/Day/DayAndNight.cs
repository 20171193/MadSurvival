using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jungmin
{
	public class DayAndNight : MonoBehaviour
	{
		[SerializeField] Gradient colorGradation;

		[SerializeField] float secondPerRealTimeSecond; // 게임 세계의 1초는 
		[SerializeField] float nightFogDensity; //밤 상태에서 Fog 밀도
		float dayForDensity; // 낮 상태에서 Fog 밀도 
		[SerializeField] float currentFogDensity; //계산
		[SerializeField] float fogDensityCalc; // 증감량 비용
		public bool IsNight = false;
		[SerializeField] public float dayTimer;
		Coroutine coroutine;
		Coroutine exitNightRoutine;
		Coroutine exitFogRoutine;
		Coroutine enterNightRoutine;
		Coroutine enterFogRoutine;

		public float resetTimeValue;
		public int days;
		int prevDayValue;
		bool checkday; //일몰시 한번만 Days를 증감시키는 메소드를 한번만 시도하게 하는 메소드
		/* 
		 * 해가 뜰때 isnight = false; 및 라이트 위치 서서히 조정 메소드
		 * 낮 카운트 다운 시작. 낮 동안은 해가 움직이지 않음.
		 * 낮 카운트 다운이 끝날 경우
		 * isnight = false로 변경
		 * 밤으로 라이트 위치 조정 메소드
		 * 
		 * 
		 */
		public UnityAction OnNight;

        private void Awake()
        {
            //resetTimeValue = dayTimer;
            //prevDayValue = -1;
            //dayForDensity = RenderSettings.fogDensity;
        }

  //      IEnumerator DayTimeCoroutine()
		//{
		//	while(dayTimer > 0f)
		//	{
		//		dayTimer -= Time.deltaTime;
		//		yield return null;
		//	}
		//	dayTimer = 0f;
		//	OnEnterNight();
  //          yield return null;
		//}

		// --------DAY ENTER EXIT
		public void OnExitNight()
		{
			// 타이머 초기화
			if (prevDayValue == 1)
			{ //Day 0에서는 타임이 초기화되지 않음.
				dayTimer = resetTimeValue;
			}
			IsNight = false;
			//exitNightRoutine = StartCoroutine(DayTimeCoroutine());
            //exitFogRoutine = StartCoroutine(ExitFogRoutine());
			prevDayValue = IsNight ? 1 : 0;

            RenderSettings.fogDensity = 0f;
        }
		public void OnInFog()
		{
			//IsNight = true;
			//prevDayValue = IsNight ? 1 : 0;
			//OnNight?.Invoke();
			enterFogRoutine = StartCoroutine(InFogRoutine());
		}
		// -------FOG ENTER EXIT

		// 심재천 수정
		IEnumerator InFogRoutine()
		{
            float rate = 0f;
            while (rate < 1)
            {
                rate = GameFlowController.Inst.DayTime / GameFlowController.Inst.DayChangeTime;
                RenderSettings.fogDensity = Mathf.Lerp(0, nightFogDensity, rate);
                yield return null;
            }

            RenderSettings.fogDensity = nightFogDensity;
            yield return null;
        }
		//IEnumerator OutFogRoutine()
		//{
		//	float rate = 0f;
  //          while (rate < 1)
  //          {
		//		rate = GameFlowController.Inst.DayTime / GameFlowController.Inst.DayChangeTime;
  //              RenderSettings.fogDensity = Mathf.Lerp(nightFogDensity,0, rate);
  //              yield return null;
  //          }

		//	RenderSettings.fogDensity = 0f;
  //          yield return null;
  //      }

		//IEnumerator EnterFogRoutine()
		//{
		//	while (currentFogDensity <= nightFogDensity )
		//	{
		//		EnterFog();
		//		yield return null;
		//	}
		//}
		//IEnumerator ExitFogRoutine()
		//{
		//	while ( currentFogDensity > dayForDensity )
		//	{
		//		ExitFog();
		//		yield return null;
		//	}
		//}
		//void ExitFog()
		//{
		//	currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
		//	RenderSettings.fogDensity = currentFogDensity;
		//}
		//void EnterFog()
		//{
		//	currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
		//	RenderSettings.fogDensity = currentFogDensity;
		//}
	}
}
