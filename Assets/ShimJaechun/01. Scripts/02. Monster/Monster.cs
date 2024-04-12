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
    public class Monster : PooledObject, ITileable, IDamageable, IKnockbackable
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

        [SerializeField]
        private ExplosionInvoker explosionInvoker;

        [SerializeField]
        private DropItem meat;

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
        public void TakeDamage(float value)
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
            }
        }

        public void Knockback(float power, float time, Vector3 suspectPos)
        {
            knockBackTimer = StartCoroutine(KnockBackRoutine(power, time, suspectPos));
        }

        IEnumerator KnockBackRoutine(float power, float time, Vector3 suspectPos)
        {
            // 네비메시 비활성화
            // 물리 이동으로 전환

            Vector3 dir = transform.position - suspectPos;

            agent.enabled = false;
            rigid.AddForce(dir * power, ForceMode.Impulse);

            yield return new WaitForSeconds(time);

            // 원복
            rigid.velocity = Vector3.zero;
            agent.enabled = true;

            // 넉백 후 딜레이
            agent.isStopped = true;
            yield return new WaitForSeconds(0.2f);
            agent.isStopped = false;
        }
        #endregion

        public void DropItem()
        {
            // 아이템 확률 적용
            if (stat.DropMeatPercent == 0) return;

            float rand = Random.Range(1, 10) / 10f;
            if (rand > stat.DropMeatPercent) return;

            Manager.Pool.GetPool(meat, transform.position + Vector3.up, Quaternion.identity);

            ExplosionInvoker invoker = (ExplosionInvoker)Manager.Pool.GetPool(explosionInvoker, transform.position, Quaternion.identity);
            invoker.OnExplosion();
        }

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
            ScoreboardInvoker.Instance.killMonster?.Invoke(ScoreType.Monster);
        }
    }
}