using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Jc
{
    public class Monster : PooledObject, ITileable, IDamageable
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private Rigidbody rigid;

        [SerializeField]
        private NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        [SerializeField]
        private Animator anim;
        public Animator Anim { get { return anim; } }

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        private MonsterFSM fsm;
        public MonsterFSM FSM { get { return fsm; } }

        [SerializeField]
        private MonsterStat stat;
        public MonsterStat Stat { get { return stat; } }

        [SerializeField]
        private MonsterDetecter detecter;
        public MonsterDetecter Detecter { get { return detecter; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [Header("꼭 지정해야하는 부분. 이름으로 Find")]
        [SerializeField]
        protected string monsterName;
        public string MonsterName { get { return monsterName; } }

        [SerializeField]
        private float knockBackPower;
        public float KnockBackPower { get { return knockBackPower; } }

        [SerializeField]
        private float knockBackTime;
        public float KnockBackTime { get { return knockBackTime; } }

        private Coroutine knockBackTimer;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        public UnityAction<Monster> OnMonsterDie;
        public string currentState;

        // 간략화
        public NavigationManager Navi => Manager.Navi;


        private void Awake()
        {
            fsm.CreateFSM(this);
        }

        private void OnEnable()
        {
            // 플레이어의 타일 위치가 변경될 경우 발생할 액션
            Navi.OnChangePlayerGround += OnChangeTarget;
            // 게임맵 할당
            detecter.PlayerGround = Navi.OnPlayerGround;

            // 상태변경
            fsm.ChangeState("Idle");
        }

        private void Update()
        {
            currentState = fsm.currentState;
        }

        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        #region 데미지 처리
        // 데미지 처리
        public void TakeDamage(float value, Vector3 suspectPos)
        {
            // 데미지값 처리
            float damage = value - stat.AMR;
            if (damage <= 0) return;

            // 사망처리
            if(stat.OwnHp < damage)
            {
                fsm.ChangeState("Die");
            }
            // 데미지 처리, 넉백
            else
            {
                anim.SetTrigger("OnHit");
                stat.OwnHp -= damage;
                knockBackTimer = StartCoroutine(KnockBackRoutine());
            }
        }
        IEnumerator KnockBackRoutine()
        {
            // 네비메시 비활성화
            // 물리 이동으로 전환
            agent.enabled = false;
            rigid.AddForce(transform.forward * -knockBackPower, ForceMode.Impulse);

            yield return new WaitForSeconds(KnockBackTime);

            // 원복
            rigid.velocity = Vector3.zero;
            agent.enabled = true;

            // 넉백 후 딜레이
            agent.isStopped = true;
            yield return new WaitForSeconds(0.2f);
            agent.isStopped = false;
        }
        #endregion

        #region 인터페이스 오버라이드
        // 몬스터가 타일에 진입한 경우 세팅
        public void OnTile(Ground ground)
        {
            detecter.OnGround = ground;
        }

        // 플레이어 위치가 변경될 경우 호출될 함수
        public void OnChangeTarget(Ground playerGround)
        {
            detecter.PlayerGround = playerGround;
        }
        #endregion

        public override void Release()
        {
            base.Release();
            OnMonsterDie?.Invoke(this);
        }
    }
}