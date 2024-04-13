using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Jc
{
    public enum ScoreType
    {
        Monster,
        Animal,
        Meat,
        Water,
        Tree,
        Stone
    }

    public class Scoreboard : MonoBehaviour
    {
        [Header("죽인 몬스터 수")]
        public int killMonster;
        [Header("죽인 동물 수")]
        public int killAnimal;
        [Header("먹은 고기 갯수")]
        public int eatMeat;
        [Header("물 마신 횟수")]
        public int drinkWater;
        [Header("캔 나무의 수")]
        public int digTree;
        [Header("캔 돌의 수")]
        public int digStone;
        [Header("플레이 일 수 ")]
        public int day;
        [Header("전체 플레이 시간")]
        public float totalTime;

        public void UpdateScore(ScoreType type)
        {
            switch (type)
            {
                case ScoreType.Monster:
                    killMonster++;
                    break;
                case ScoreType.Animal:
                    killAnimal++;
                    break;
                case ScoreType.Meat:
                    eatMeat++;
                    break;
                case ScoreType.Water:
                    drinkWater++;
                    break;
                case ScoreType.Tree:
                    digTree++;
                    break;
                case ScoreType.Stone:
                    digStone++;
                    break;
            }
        }
    }
}
