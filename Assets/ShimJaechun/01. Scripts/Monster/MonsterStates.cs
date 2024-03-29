using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class MonsterBaseState : BaseState
    {
        protected Monster owner;
    }

    // 대기 상태
    public class MonsterIdle : MonsterBaseState
    {
        public MonsterIdle(Monster owner)
        {
            this.owner = owner;
        }

        public override 
    }

    public class MonsterTracking : MonsterBaseState
    {
        public MonsterTracking(Monster owner)
        {
            this.owner = owner;
        }
    }

    public class MonsterAttack : MonsterBaseState
    {
        public MonsterAttack(Monster owner)
        {
            this.owner = owner;
        }
    }

    public class MonsterDie : MonsterBaseState
    {
        public MonsterDie(Monster owner)
        {
            this.owner = owner;
        }
    }
}
