using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField]
        private GameObject tutorialPopUp;

        [SerializeField]
        private GameObject[] tutorialImages;

        #region Æ©Åä¸®¾ó
        private int currentIndex = 0;

        private void OnEnable()
        {
            Manager.Sound.PlayBGM(0);
        }
        private void OnDisable()
        {
            Manager.Sound.StopBGM();
        }

        public void OpenTutorial()
        {
            Manager.Sound.PlaySFX(0);
            tutorialPopUp.SetActive(true);
            tutorialImages[currentIndex].SetActive(true);
        }

        public void OnClickQuitButton()
        {
            Manager.Sound.PlaySFX(0);
            tutorialImages[currentIndex].SetActive(false);
            currentIndex = 0;
            tutorialPopUp.SetActive(false);
        }
        public void OnClickPrevButton()
        {
            Manager.Sound.PlaySFX(0);
            if (currentIndex == 0) return;

            tutorialImages[currentIndex].SetActive(false);
            tutorialImages[--currentIndex].SetActive(true);
        }
        public void OnClickNextButton()
        {
            if(currentIndex == tutorialImages.Length -1)
            {
                OnClickQuitButton();
                return;
            }
            Manager.Sound.PlaySFX(0);
            tutorialImages[currentIndex].SetActive(false);
            tutorialImages[++currentIndex].SetActive(true);
        }
        #endregion

        public void OnClickStartButton()
        {
            Manager.Sound.PlaySFX(1);
            Manager.Scene.LoadScene(SceneNameType.InGame);
        }
    }
}
