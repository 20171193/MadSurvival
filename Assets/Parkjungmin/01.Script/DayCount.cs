using jungmin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCountUI : MonoBehaviour
{
	[SerializeField] GameObject timer;
	TMP_Text text;

	private void Start()
	{
		text = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		text.text = ( ( int ) ( timer.GetComponent<DayAndNight>().days ) ).ToString();
	}
}