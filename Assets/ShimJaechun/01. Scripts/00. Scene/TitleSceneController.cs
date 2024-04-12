using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class TitleScene : MonoBehaviour
    {
        public void OnClickStartButton()
        {
            Manager.Scene.LoadScene(SceneNameType.InGame);
        }
    }
}
