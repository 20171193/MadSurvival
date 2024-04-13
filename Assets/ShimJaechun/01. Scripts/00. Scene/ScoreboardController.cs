using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    public enum TextOrder
    {
        Day,
        TotalTime,
        Animal,
        Monster,
        Meat,
        Water,
        Tree,
        Stone
    }

    public class ScoreboardController : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        [SerializeField]
        private Scoreboard scoreboard;

        [Header("스코어보드 텍스트")]
        [SerializeField]
        private TextMeshProUGUI[] texts;

        public void OnRenderScoreboard()
        {
            LoadScore();

            anim.SetTrigger("OnRender");
        }

        private void LoadScore()
        {
            texts[(int)TextOrder.Day].text = GameFlowController.Inst.Day.ToString();
            HMSTime hmsTime = new HMSTime((int)GameFlowController.Inst.TotalTime);
            texts[(int)TextOrder.TotalTime].text = $"{hmsTime.hour}h {hmsTime.minute}m {hmsTime.second}s";
            texts[(int)TextOrder.Animal].text = scoreboard.killAnimal.ToString();
            texts[(int)TextOrder.Monster].text = scoreboard.killMonster.ToString();
            texts[(int)TextOrder.Meat].text = scoreboard.eatMeat.ToString();
            texts[(int)TextOrder.Water].text = scoreboard.drinkWater.ToString();
            texts[(int)TextOrder.Tree].text = scoreboard.digTree.ToString();
            texts[(int)TextOrder.Stone].text = scoreboard.digStone.ToString();

        }

        public void OnClickTitleButton()
        {
            Manager.Scene.LoadScene(SceneNameType.Title);
        }
    }
}
