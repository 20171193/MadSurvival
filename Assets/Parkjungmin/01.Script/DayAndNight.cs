using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jungmin
{
	public class DayAndNight : MonoBehaviour
	{
		[SerializeField] Gradient colorGradation;

		[SerializeField] float secondPerRealTimeSecond; // ���� ������ 1�ʴ� 
		[SerializeField] float nightFogDensity; //�� ���¿��� Fog �е�
		float dayForDensity; // �� ���¿��� Fog �е� 
		[SerializeField] float currentFogDensity; //���
		[SerializeField] float fogDensityCalc; // ������ ���
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
		bool checkday; //�ϸ��� �ѹ��� Days�� ������Ű�� �޼ҵ带 �ѹ��� �õ��ϰ� �ϴ� �޼ҵ�
		/* 
		 * �ذ� �㶧 isnight = false; �� ����Ʈ ��ġ ������ ���� �޼ҵ�
		 * �� ī��Ʈ �ٿ� ����. �� ������ �ذ� �������� ����.
		 * �� ī��Ʈ �ٿ��� ���� ���
		 * isnight = false�� ����
		 * ������ ����Ʈ ��ġ ���� �޼ҵ�
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
			// Ÿ�̸� �ʱ�ȭ
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
		//	//if ( IsNight ) //�ϸ� �� (���� �Ȱ�)���� �þ. 
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
		//	//else //���� �� ���� �پ��
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
		//		//if ( transform.rotation.eulerAngles.x != 70f ) //70�� �϶� ����
		//		//	transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
		//		ChangeFog();
		//	}
		//	else
		//	{
		//		//if ( transform.rotation.eulerAngles.x != 270 ) //270�� �϶� ������ ��.
		//		//{
		//		//	transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
		//		//}
		//		ChangeFog();
		//	}

		//}
	}
}
