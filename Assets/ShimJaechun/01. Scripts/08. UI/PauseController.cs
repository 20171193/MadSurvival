using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Jc
{
    public class PauseController : MonoBehaviour
    {
        [Header("�Ͻ����� �˾�")]
        [SerializeField]
        private GameObject pausePopUp;

        [Header("������ �����̴�")]
        [SerializeField]
        private Slider masterSlider;
        [Header("����� �����̴�")]
        [SerializeField]
        private Slider bgmSlider;
        [Header("ȿ���� �����̴�")]
        [SerializeField]
        private Slider sfxSlider;
        public void OnClickPauseButton()
        {
            pausePopUp.SetActive(!pausePopUp.activeSelf);
            Time.timeScale = pausePopUp.activeSelf ? 0f : 1f;
        }

        public void OnClickTitleButton()
        {
            Time.timeScale = 1f;
            Manager.Scene.LoadScene(SceneNameType.Title);
        }

        public void OnChangeMasterValue()
        {
            Manager.Sound.AudioMixer.SetFloat("Master", masterSlider.value);
        }
        public void OnChangeBGMValue()
        {
            Manager.Sound.AudioMixer.SetFloat("BGM",bgmSlider.value);
        }
        public void OnChangeSFXValue()
        {
            Manager.Sound.AudioMixer.SetFloat("SFX", sfxSlider.value);
        }
    }
}
