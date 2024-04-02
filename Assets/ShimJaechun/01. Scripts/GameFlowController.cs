using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace Jc
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField]
        private int day = 0;
        public int Day { get { return day; } set { day = value; } }

        [SerializeField]
        private DayAndNight dayController;
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        private void Awake()
        {
            dayController.OnNight += EnterNight;
        }

        private void OnEnable()
        {
            ExitNight();
        }

        public void EnterNight()
        {
            // ���� ����
            //��-> �� ����
            // ���� ����
            monsterSpawner.OnSpawn(day);
        }
        public void ExitNight()
        {
			// �� -> �� ����
			dayController.OnExitNight();
			// ��¥ ī����
			day++;
        }
    }
}
