using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jc
{
    public class SliderController : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        public void UpdateSlider(float value, float maxValue)
        {
            slider.value = value / maxValue;
        }
    }
}
