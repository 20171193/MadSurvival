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
        [Header("���� ���� ��")]
        public int killMonster;
        [Header("���� ���� ��")]
        public int killAnimal;
        [Header("���� ��� ����")]
        public int eatMeat;
        [Header("�� ���� Ƚ��")]
        public int drinkWater;
        [Header("ĵ ������ ��")]
        public int digTree;
        [Header("ĵ ���� ��")]
        public int digStone;
        [Header("�÷��� �� �� ")]
        public int day;
        [Header("��ü �÷��� �ð�")]
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
