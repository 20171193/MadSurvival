using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void OnRenderScoreboard()
    {
        Time.timeScale = 0f;
        anim.SetTrigger("OnRender");
    }

    public void OnClickTitleButton()
    {

    }
}
