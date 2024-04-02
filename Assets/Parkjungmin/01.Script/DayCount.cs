using jungmin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCountUI : MonoBehaviour
{
	[SerializeField] GameObject SunObject;
	TMP_Text text;

	private void Start()
	{
		text = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		text.text = ( ( int ) ( SunObject.GetComponent<DayAndNight>().days ) ).ToString();
	}
}
