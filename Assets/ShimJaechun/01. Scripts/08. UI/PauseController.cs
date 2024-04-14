using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PauseController : MonoBehaviour
    {
        [Header("�Ͻ����� �˾�")]
        [SerializeField]
        private GameObject pausePopUp;

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
    }
}
