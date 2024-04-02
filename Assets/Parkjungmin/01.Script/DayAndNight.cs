using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jungmin{
public class DayAndNight : MonoBehaviour
{
	[SerializeField] float secondPerRealTimeSecond; // ���� ������ 1�ʴ� 
	[SerializeField] float nightFogDensity; //�� ���¿��� Fog �е�
	float dayForDensity; // �� ���¿��� Fog �е� 
	[SerializeField] float currentFogDensity; //���
	[SerializeField]float fogDensityCalc; // ������ ���
	public bool IsNight = false;
	[SerializeField] public float dayTimer;
	[SerializeField] Coroutine coroutine;
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

	IEnumerator DayTimeCoroutine()
	{
		dayTimer -= Time.deltaTime;
		if(dayTimer <= 0 )
		{
			OnNight?.Invoke();
			IsNight = true;
				Debug.Log("its night");
		}
		yield return null;
	}
	private void Start()
	{
		daytimer_ = dayTimer;
		prevDayValue = -1;
		dayForDensity = RenderSettings.fogDensity;
	}

	private void Update()
	{
		MovingSun();
	}
	void MovingSun()
		{
			if ( IsNight )
			{
				//if ( transform.rotation.x < 70f) //70�� �϶� ����
				//	transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
				ChangeFog();
			}
			else
			{
				//if( transform.rotation.x != 270) //270�� �϶� ������ ��.
				//{
				//	transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
				//}
				ChangeFog();
			}

	}
		void ChangeFog()
		{
			if ( IsNight ) //�ϸ� �� (���� �Ȱ�)���� �þ. 
			{
				StopCoroutine(coroutine);
				dayTimer = daytimer_;
				if ( currentFogDensity <= nightFogDensity )
				{
					currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
					RenderSettings.fogDensity = currentFogDensity;
				}
				checkday = true;
			}
			else //���� �� ���� �پ��
			{
				if ( checkday )
				{
					prevDayValue = IsNight ? 1 : 0;
					if ( prevDayValue == 0 && !IsNight )
					{
						days++;
						checkday = false;
					}

				}
				if ( currentFogDensity > dayForDensity )
				{
					currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
					RenderSettings.fogDensity = currentFogDensity;
					
				}
				coroutine = StartCoroutine(DayTimeCoroutine());

			}
		}

}
}
