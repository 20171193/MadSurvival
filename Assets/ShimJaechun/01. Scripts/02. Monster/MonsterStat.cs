using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class MonsterStat : MonoBehaviour
    {
        [SerializeField]
        private Monster owner;

        [Header("스텟")]
        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } set { speed = value; owner.Agent.speed = value; } }

        [SerializeField]
        private float atk;
        // 프로퍼티 추가 예정
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
        // 몬스터 초기설정 (데이터 로딩)
        private void InitSetting()
        {
            if (!Manager.Data.monsterDataDic.ContainsKey(owner.MonsterName))
            {
                Debug.Log($"{owner.MonsterName} : 의 데이터가 없습니다.");
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
