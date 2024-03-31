using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Jc
{
    public class Monster : MonoBehaviour, ITileable, IDamageable
    {
        [Header("Components")]
        [Space(2)]
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
        private MonsterDetecter detecter;
        public MonsterDetecter Detecter { get { return detecter; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } set { speed = value; agent.speed = value; } }

        [SerializeField]
        private float hp;
        // 프로퍼티 추가 예정

        [SerializeField]
        private float atk;
        // 프로퍼티 추가 예정

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        // 전체적인 게임 맵
        [SerializeField]
        private List<GroundList> gameMap;
        public List<GroundList> GameMap { get { return gameMap; } }

        // 몬스터가 스폰된 타일
        [SerializeField]
        private Ground onGround;
        public Ground OnGround { get { return onGround; } }

        // 플레이어가 위치한 타일
        private Ground playerGround;
        public Ground PlayerGround { get { return playerGround; } }

        // 현재 추격하는 대상이 플레이어인지 체크
        private bool isTrackingPlayer;
        public bool IsTrackingPlayer { get { return isTrackingPlayer; } }

        // 간략화
        public NavigationManager Navi => Manager.Navi;

        private void Awake()
        {
            fsm.CreateFSM(this);
            detecter.OnTrigger += DetectTarget;
        }

        private void OnEnable()
        {
            // 추후 CSV 데이터를 먼저 로드해서 정해진 속도로 할당하기.

            // 속도 할당
            agent.speed = speed;

            // 플레이어의 타일 위치가 변경될 경우 발생할 액션
            Navi.OnChangePlayerGround += OnChangeTarget;
            // 게임맵 할당
            gameMap = Navi.gameMap;
            playerGround = Navi.OnPlayerGround;
        }

        private void Update()
        {
            anim.SetFloat("MoveSpeed", agent.velocity.magnitude);
        }

        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // 몬스터 전용 트리거의 액션으로 호출
        // 트래킹 중 주변 객체와 닿았을 경우 공격 
        public void DetectTarget(GameObject target)
        {
            // 플레이어 탐지
            if (isTrackingPlayer && target.tag == "Player" ||
                !isTrackingPlayer && Manager.Layer.wallLM.Contain(target.layer))
            {
                fsm.ChangeState("Attack");
            }
        }

        // 데미지 처리
        public void TakeDamage(float damage, Vector3 suspectPos)
        {
            // 데미지값 처리

            // 넉백 처리
        }

        // 몬스터가 타일에 진입한 경우 세팅
        public void OnTile(Ground ground)
        {
            onGround = ground;
        }
        // 플레이어 위치가 변경될 경우 호출될 함수
        public void OnChangeTarget(Ground playerGround)
        {
            this.playerGround = playerGround;
        }

        #region 몬스터 추격 알고리즘
        // 목적지 세팅
        // 플레이어가 벽으로 둘러싸여 있는지 체크
        //  - true : 가장 가까운 벽으로 이동
        //  - false : 플레이어로 이동
        public void Tracking(Ground playerGround)
        {
            // 예외처리 : 플레이어가 타일 위에 위치하지않은 경우
            if (playerGround == null)
            {
                Debug.Log("플레이어가 위치한 타일이 존재하지 않습니다.");
                return;
            }
            // 지정한 목표 (플레이어)
            Ground originTarget = playerGround;
            // 탐색 결과 목표
            Ground resultTarget = TargetSetting();
            agent.destination = resultTarget.transform.position;

            // 탐색 결과가 플레이어일 경우 true
            isTrackingPlayer = originTarget == resultTarget;
        }

        private Ground TargetSetting()
        {
            int zPos = playerGround.Pos.z;
            int xPos = playerGround.Pos.x;

            // 플레이어가 진지를 구축할 수 없는 영역에 존재하는 경우
            // -> 목적지를 플레이어가 위치한 타일로 설정.
            if (zPos < Navi.cornerTL.z || zPos > Navi.cornerBL.z || xPos < Navi.cornerTL.x || xPos > Navi.cornerTR.x)
                return playerGround;

            // 플레이어 주변이 벽으로 둘러싸이지 않은 경우
            if (!PlayerInBaseCamp())
                return playerGround;
            // 플레이어 주변이 벽으로 둘러싸인 경우
            // 가장 가까운 벽을 찾아 추격
            return GetNearWall();
        }

        // 플레이어가 벽으로 둘러싸여있는지 체크
        // 진지를 구축할 수 있는 좌표에서 탐색 (플레이어 기준 BFS)
        private bool PlayerInBaseCamp()
        {
            // 플레이어가 진지를 구축할 수 있는 영역에 존재하는 경우
            // -> 체크 : 플레이어가 벽으로 둘러싸여있는지?

            // BFS 탐색
            Queue<GroundPos> q = new Queue<GroundPos>();
            // 방문확인 배열 생성 (원점 기준)
            bool[,] visitied = new bool[Navi.mapZsize/3,Navi.mapXsize/3];
            int resol = Navi.mapZsize / 3 - 1;
            // 4방향 탐색할 방향 설정
            int[] dz = { 0, 0, 1, -1 };
            int[] dx = { 1, -1, 0, 0 };
            q.Enqueue(playerGround.Pos);
            // 방문배열 확인을 위해 그라운드 위치를 원점좌표로 변환
            visitied[playerGround.Pos.z - resol, playerGround.Pos.x - resol] = true;

            while(q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for(int i =0; i<4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dx[i] + curPos.x;

                    // 예외처리 
                    if (nz > Navi.cornerTL.z || nz < Navi.cornerBL.z || nx < Navi.cornerTL.x || nx > Navi.cornerTR.x) continue;
                    if (visitied[nz - resol, nx - resol]) continue;
                    if (Navi.gameMap[nz].groundList[nx].type != GroundType.Buildable) continue;

                    // 진지를 구축할 수 있는 끝점 좌표에 도달한 경우 (벽이 뚫린 경우)
                    if (nz >= Navi.cornerTL.z || nz <= Navi.cornerBL.z || nx <= Navi.cornerTL.x || nx >= Navi.cornerTR.x)
                        return true;

                    q.Enqueue(new GroundPos(nz, nx));
                    visitied[nz-resol, nx-resol] = true;
                }
            }
            // 벽으로 둘러싸인 경우
            return false;
        }

        // 레이캐스트를 통해 가장 가까운 벽 찾기
        private Ground GetNearWall()
        {
            // 현재 위치에서 가장 가까운 벽을 목적지로 설정
            // 현재위치 -> 플레이어위치 레이캐스팅 [LayerMask = 벽]
            Vector3 startPos = new Vector3(onGround.transform.position.x, 0.2f, onGround.transform.position.z);
            Vector3 endPos = new Vector3(playerGround.transform.position.x, 0.2f, onGround.transform.position.z);
            Debug.DrawLine(startPos, endPos, Color.red, 0.5f);

            // 가장 가까운 벽으로 이동 
            if (Physics.Raycast(new Ray(startPos, endPos), out RaycastHit hitInfo, (endPos - startPos).magnitude, Manager.Layer.wallLM))
            {
                Wall targetWall = hitInfo.transform.GetComponent<Wall>();
                return targetWall?.OnGround;
            }

            Debug.Log($"{this.gameObject} can't find wall");
            return null;
        }
        #endregion
    }
}