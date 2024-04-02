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

		float daytimer_;
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
            daytimer_ = dayTimer;
            prevDayValue = -1;
            dayForDensity = RenderSettings.fogDensity;
        }

        IEnumerator DayTimeCoroutine()
		{
			while(dayTimer > 0f)
			{
				dayTimer -= Time.deltaTime;
				yield return null;
			}
			dayTimer = 0f;
			OnEnterNight();
            yield return null;
		}

		// --------DAY ENTER EXIT
		public void OnExitNight()
		{
			// 타이머 초기화
			dayTimer = daytimer_;

            IsNight = false;
			exitNightRoutine = StartCoroutine(DayTimeCoroutine());
            exitFogRoutine = StartCoroutine(ExitFogRoutine());
        }
		public void OnEnterNight()
		{
            IsNight = true;
			OnNight?.Invoke();
        }
		// -------FOG ENTER EXIT

		IEnumerator EnterFogRoutine()
		{
			while (currentFogDensity <= nightFogDensity )
			{
				EnterFog();
				Debug.Log("enterfog");
				yield return null;
			}
		}
		IEnumerator ExitFogRoutine()
		{
			while ( currentFogDensity > dayForDensity )
			{
				ExitFog();
				Debug.Log("exitfog");
				yield return null;
			}
		}
		void ExitFog()
		{
			currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
			RenderSettings.fogDensity = currentFogDensity;
		}
		void EnterFog()
		{
			currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
			RenderSettings.fogDensity = currentFogDensity;
		}

		//void ChangeFog()
		//{
		//	//if ( IsNight ) //일몰 시 (검은 안개)포그 늘어남. 
		//	//{
		//	//	StopCoroutine(coroutine);
		//	//	dayTimer = daytimer_;
		//	//	if ( currentFogDensity <= nightFogDensity )
		//	//	{
		//	//		currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
		//	//		RenderSettings.fogDensity = currentFogDensity;
		//	//	}
		//	//	checkday = true;
		//	//}
		//	//else //일출 시 포그 줄어듬
		//	//{
		//	//	if ( checkday )
		//	//	{
		//	//		prevDayValue = IsNight ? 1 : 0;
		//	//		if ( prevDayValue == 0 && !IsNight )
		//	//		{
		//	//			days++;
		//	//			checkday = false;
		//	//		}

		//	//	}
		//	//	coroutine = StartCoroutine(DayTimeCoroutine());

		//	//}
		//}
		//void MovingSun()
		//{
		//	if ( IsNight )
		//	{
		//		//if ( transform.rotation.eulerAngles.x != 70f ) //70도 일때 자정
		//		//	transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
		//		ChangeFog();
		//	}
		//	else
		//	{
		//		//if ( transform.rotation.eulerAngles.x != 270 ) //270도 일때 자정이 됨.
		//		//{
		//		//	transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
		//		//}
		//		ChangeFog();
		//	}

		//}
	}
}
