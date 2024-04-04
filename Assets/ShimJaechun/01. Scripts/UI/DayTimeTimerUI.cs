using Jc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    public class DayTimeTimerUI : TimerUI
    {
        [SerializeField]
        private TextMeshProUGUI titleTmpro;

        private Color originTitleColor;
        private Color originTimerColor;

        private void Awake()
        {
            originTitleColor = titleTmpro.color;
            originTimerColor = timerTmpro.color;
        }

        public void OnDayTimer(int day)
        {
            titleTmpro.text = $"Day{day}";
            titleTmpro.color = originTitleColor;
            timerTmpro.color = originTimerColor;
        }

        public void OffDayTimer()
        {
            titleTmpro.color = Color.gray;
            timerTmpro.color = Color.gray;
        }
    }
}
