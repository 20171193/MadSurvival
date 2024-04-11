using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class MonsterStat : MonoBehaviour
    {
        [SerializeField]
        private Monster owner;

        [Header("����")]
        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } set { speed = value; owner.Agent.speed = value; } }

        [SerializeField]
        private float atk;
        // ������Ƽ �߰� ����
        public float ATK { get { return atk; } }

        [SerializeField]
        private float ats;
        public float ATS { get { return ats; } }

        [SerializeField]
        private float maxHp;
        public float MaxHp { get { return maxHp; } }
        [SerializeField]
        private float ownHp;
        public float OwnHp { get { return ownHp; } set { ownHp = value; } }

        [SerializeField]
        private float amr;
        public float AMR { get { return amr; } }

        [SerializeField]
        private float dropMeatPercent;
        public float DropMeatPercent { get { return dropMeatPercent; } }

        private void OnEnable()
        {
            InitSetting();
        }
        // ���� �ʱ⼳�� (������ �ε�)
        private void InitSetting()
        {
            if (!Manager.Data.monsterDataDic.ContainsKey(owner.MonsterName))
            {
                Debug.Log($"{owner.MonsterName} : �� �����Ͱ� �����ϴ�.");
                return;
            }

            MonsterData loadedData = Manager.Data.monsterDataDic[owner.MonsterName];
            Speed = loadedData.speed;
            atk = loadedData.atk;
            ats = loadedData.ats;
            maxHp = loadedData.hp;
            ownHp = maxHp;
            amr = loadedData.amr;
            dropMeatPercent = loadedData.dropMeatPercent;
        }


    }
}
