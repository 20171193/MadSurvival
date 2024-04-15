using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public enum PlayerStatType
    {
        Speed,
        Hp,
        Hunger,
        Thirst,
        Amor,
        MonsterATK,
        TreeATK,
        StoneATK
    }

    public class DebugController : MonoBehaviour
    {
        [SerializeField]
        private PlayerStat stat;

        [SerializeField]
        private TextMeshProUGUI speedTXT;
        [SerializeField]
        private TextMeshProUGUI hpTXT;
        [SerializeField]
        private TextMeshProUGUI hungerTXT;
        [SerializeField]
        private TextMeshProUGUI thirstTXT;
        [SerializeField]
        private TextMeshProUGUI amorTXT;
        [SerializeField]
        private TextMeshProUGUI monsteratkTXT;
        [SerializeField]
        private TextMeshProUGUI treeatkTXT;
        [SerializeField]
        private TextMeshProUGUI stoneatkTXT;

        private void UpdateValue()
        {
            speedTXT.text = stat.MaxSpeed.ToString();
            hpTXT.text = stat.OwnHp.ToString();
            hungerTXT.text = stat.OwnHunger.ToString();
            thirstTXT.text = stat.OwnThirst.ToString();
            amorTXT.text = stat.AMR.ToString();
            monsteratkTXT.text = (stat.BaseMonsterATK + stat.MonsterATK).ToString();
            treeatkTXT.text = (stat.BaseTreeAtk + stat.TreeATK).ToString();
            stoneatkTXT.text = (stat.BaseStoneAtk + stat.StoneATK).ToString();
        }

        private void OnEnable()
        {
            UpdateValue();
        }

        public void OnClickUpButton(PlayerStatType type)
        {
            switch (type)
            {
                case PlayerStatType.Speed:
                    stat.MaxSpeed += 1f;
                    break;
                case PlayerStatType.Hp:
                    stat.OwnHp += 5f;
                    break;
                case PlayerStatType.Hunger:
                    stat.OwnHunger += 5f;
                    break;
                case PlayerStatType.Thirst:
                    stat.OwnThirst += 5f;
                    break;
                case PlayerStatType.Amor:
                    stat.AMR += 1f;
                    break;
                case PlayerStatType.MonsterATK:
                    stat.MonsterATK += 1f;
                    break;
                case PlayerStatType.TreeATK:
                    stat.TreeATK += 1f;
                    break;
                case PlayerStatType.StoneATK:
                    stat.StoneATK += 1f;
                    break;
            }
            UpdateValue();
        }
        public void OnClickDownButton(PlayerStatType type)
        {
            switch (type)
            {
                case PlayerStatType.Speed:
                    stat.MaxSpeed -= 1f;
                    break;
                case PlayerStatType.Hp:
                    stat.OwnHp -= 5f;
                    break;
                case PlayerStatType.Hunger:
                    stat.OwnHunger -= 5f;
                    break;
                case PlayerStatType.Thirst:
                    stat.OwnThirst -= 5f;
                    break;
                case PlayerStatType.Amor:
                    stat.AMR -= 1f;
                    break;
                case PlayerStatType.MonsterATK:
                    stat.MonsterATK -= 1f;
                    break;
                case PlayerStatType.TreeATK:
                    stat.TreeATK -= 1f;
                    break;
                case PlayerStatType.StoneATK:
                    stat.StoneATK -= 1f;
                    break;
            }
            UpdateValue();
        }
    }
}
