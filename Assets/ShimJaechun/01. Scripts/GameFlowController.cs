using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField]
        private int day;
        public int Day { get { return day; } set { day = value; } }

        [SerializeField]
        private DayAndNight dayController;
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        public void EnterNight()
        {
            // ���� ����

            // ���� ����
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
            // �� -> �� ����
            dayController.IsNight = true;
            // ��¥ ī����
            day++;
        }
    }
}
