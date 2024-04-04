using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Jc
{
    public struct HMSTime
    {
        public string hour;
        public string minute;
        public string second;

        public HMSTime(int value)
        {
            int tHour = value / 3600;
            int tMinute = value % 3600 / 60;
            int tSecond = value % 3600 % 60;

            hour = tHour < 10 ? $"0{tHour}" : $"{tHour}";
            minute = tMinute < 10 ? $"0{tMinute}" : $"{tMinute}";
            second = tSecond < 10 ? $"0{tSecond}" : $"{tSecond}";
        }
    }

    public class TimerUI : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI timerTmpro;

        public void UpdateTimer(int value)
        {
            HMSTime hmsTime = new HMSTime(value);
            timerTmpro.text = $"{hmsTime.hour}h {hmsTime.minute}m {hmsTime.second}s";
        }
    }
}
