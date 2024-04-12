using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jc
{
    public class StaminaSlider : SliderController
    {
        [SerializeField]
        private Image fillImage;

        private Color originColor;
        private Coroutine exhaustRoutine;

        private void Awake()
        {
            originColor = fillImage.color;
        }

        public void EnterExhaust()
        {
            if (exhaustRoutine == null)
                exhaustRoutine = StartCoroutine(ExhaustRoutine());
        }
        public void ExitExhaust() 
        {
            if (exhaustRoutine != null)
            {
                StopCoroutine(exhaustRoutine);
                exhaustRoutine = null;
                fillImage.color = originColor;
            }
        }

        IEnumerator ExhaustRoutine()
        {
            bool isIncrese = false;
            float rate = 0f;
            Color curColor = Color.red;
            while(true)
            {
                if(isIncrese)
                    fillImage.color = Color.Lerp(curColor, Color.red, rate);
                else
                    fillImage.color = Color.Lerp(curColor, Color.white, rate);

                rate += 0.1f;
                if (rate >= 0.5f)
                {
                    isIncrese = !isIncrese;
                    rate = 0f;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

