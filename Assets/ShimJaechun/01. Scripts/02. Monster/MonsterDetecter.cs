using Jc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Jc
{
    public class MonsterDetecter : MonoBehaviour
    {
        [SerializeField]
        private Monster owner;

        // 전체적인 게임 맵
        [SerializeField]
        private List<GroundList> gameMap;
        public List<GroundList> GameMap { get { return gameMap; } }

        // 몬스터가 스폰된 타일
        [SerializeField]
        private Ground onGround;
        public Ground OnGround { get { return onGround; } set { onGround = value; } }

        // 플레이어가 위치한 타일
        [SerializeField]
        private Ground playerGround;
        public Ground PlayerGround { get { return playerGround; } set { playerGround = value; } }

        [SerializeField]
        // 현재 추격하는 대상이 플레이어인지 체크
        private bool isTrackingPlayer;
        public bool IsTrackingPlayer { get { return isTrackingPlayer; } }

        [SerializeField]
        private List<GameObject> targetList = new List<GameObject>();
        public List<GameObject> TargetList { get { return targetList; } }

        [SerializeField]
        private GameObject currentTarget = null;
        public GameObject CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }

        // 간략화
        public NavigationManager Navi => Manager.Navi;

        // 공격을 가할 수 있는 오브젝트를 탐지
        // 매개변수 : 객체, 레이어 
        public UnityAction<GameObject> OnTrigger;
        public UnityAction<GameObject> OffTrigger;

        private void OnEnable()
        {
            gameMap = Navi.gameMap;
            Navi.OnChangePlayerGround += OnChangePlayerGround;
        }
        private void OnDisable()
        {
            Navi.OnChangePlayerGround -= OnChangePlayerGround;
            // 타깃 리스트 클리어
            targetList.Clear();
        }

        #region 몬스터 추격 알고리즘 : 추격대상 = Player
        // 플레이어 위치가 변경될 경우 호출될 함수
        public void OnChangePlayerGround(Ground playerGround)
        {
            this.playerGround = playerGround;
            // 현재 추격중일 경우 변경된 위치로 추격
            if(owner.FSM.FSM.CurState == "Tracking")
                owner.Agent.destination = playerGround.transform.position;
        }

        public void RemoveList(GameObject target)
        {
            Debug.Log($"{target} : 이 파괴되었습니다.");
            targetList.Remove(target);
            CheckTarget();
        }
        private GameObject CheckTarget()
        {
            // 변경할 타깃이 없는경우
            if (targetList.Count < 1)
            {
                currentTarget = null;
                return null;
            }
            // 타깃이 있는 경우
            else
                return targetList[0];
        }

        #endregion

        #region 몬스터 추격 알고리즘
        // 목적지 세팅
        // 플레이어가 벽으로 둘러싸여 있는지 체크
        //  - true : 가장 가까운 벽으로 이동
        //  - false : 플레이어로 이동
        public void Tracking(Ground playerGround)
        {
            if (owner.Agent.enabled == false) return;
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
            owner.Agent.destination = resultTarget.transform.position;

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
            bool[,] visitied = new bool[Navi.mapZsize / 3 + 1, Navi.mapXsize / 3 + 1];
            int resol = Navi.mapZsize / 3 - 1;
            // 4방향 탐색할 방향 설정
            int[] dz = { 0, 0, 1, -1 };
            int[] dx = { 1, -1, 0, 0 };
            q.Enqueue(playerGround.Pos);
            // 방문배열 확인을 위해 그라운드 위치를 원점좌표로 변환
            visitied[playerGround.Pos.z - resol, playerGround.Pos.x - resol] = true;

            while (q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dx[i] + curPos.x;

                    // 예외처리 
                    if (nz > Navi.cornerBL.z || nz < Navi.cornerTL.z || nx < Navi.cornerTL.x || nx > Navi.cornerTR.x)
                        continue;
                    if (visitied[nz - resol, nx - resol])
                        continue;
                    if (Navi.gameMap[nz].groundList[nx].type != GroundType.Buildable)
                        continue;

                    // 진지를 구축할 수 있는 끝점 좌표에 도달한 경우 (벽이 뚫린 경우)
                    if (nz <= Navi.cornerTL.z || nz >= Navi.cornerBL.z || nx <= Navi.cornerTL.x || nx >= Navi.cornerTR.x)
                        return false;

                    q.Enqueue(new GroundPos(nz, nx));
                    visitied[nz - resol, nx - resol] = true;
                }
            }

            Debug.Log("플레이어는 현재 진지안에 위치해있습니다.");
            // 벽으로 둘러싸인 경우
            return true;
        }

        // 레이캐스트를 통해 가장 가까운 벽 찾기
        private Ground GetNearWall()
        {
            // 현재 위치에서 가장 가까운 벽을 목적지로 설정
            // 현재위치 -> 플레이어위치 레이캐스팅 [LayerMask = 벽]
            Vector3 startPos = new Vector3(onGround.transform.position.x, 2f, onGround.transform.position.z);
            Vector3 endPos = new Vector3(playerGround.transform.position.x, 2f, playerGround.transform.position.z);
            Debug.DrawLine(startPos, endPos, Color.red, 10f);

            // 가장 가까운 벽 찾기
            if (Physics.Raycast(startPos, (endPos - startPos).normalized, out RaycastHit hit, 200f, Manager.Layer.wallLM))
            {
                ITileable targetGround = hit.transform.GetComponent<ITileable>();
                return targetGround?.GetOnTile();
            }


            Debug.Log($"{this.gameObject} can't find wall");
            return playerGround;
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (owner.FSM.FSM.CurState == "Die" || owner.FSM.FSM.CurState == "Pooled") return;

            // 타깃으로 지정할 수 있는 오브젝트일 경우
            if (Manager.Layer.targetableLM.Contain(other.gameObject.layer))
            {
                Construct target = other.gameObject.GetComponent<Construct>();
                // 타깃이 플레이어가 아닌 벽일경우
                if (target != null)
                {
                    // 타겟리스트에 추가
                    targetList.Add(other.gameObject);
                    target.OnDestroyWall += RemoveList;
                    // 추격중이라면 공격상태로 전이
                    if (owner.FSM.FSM.CurState == "Tracking")
                    {
                        currentTarget = target.gameObject;
                        owner.FSM.ChangeState("Attack");
                    }
                }
                // 타깃이 플레이어인 경우
                else
                {
                    // 추격중이라면 공격상태로 전이
                    if (owner.FSM.FSM.CurState == "Tracking")
                    {
                        currentTarget = other.gameObject;
                        owner.FSM.ChangeState("Attack");
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (owner.FSM.FSM.CurState == "Die" || owner.FSM.FSM.CurState == "Pooled") return;

            // 지정된 타깃 해제
            if (Manager.Layer.targetableLM.Contain(other.gameObject.layer))
            {
                Construct target = other.gameObject.GetComponent<Construct>();
                // 벗어난 타깃이 벽일 경우
                if (target != null)
                {
                    target.OnDestroyWall -= RemoveList;
                    targetList.Remove(other.gameObject);
                    
                    GameObject nextTarget = CheckTarget();
                    // 인접한 다른 타깃이 없는 경우
                    // 상태전이 -> 추격
                    if (nextTarget == null)
                    {
                        currentTarget = null;
                        owner.FSM.ChangeState("Tracking");
                    }
                    // 다른 타깃이 있는 경우
                    // 타깃만 변경
                    else
                        currentTarget = nextTarget;
                }
                // 벗어난 타깃이 플레이어일 경우
                else
                {
                    currentTarget = null;
                    // 공격중이라면 추격상태로 전이
                    if (owner.FSM.FSM.CurState == "Attack")
                        owner.FSM.ChangeState("Tracking");
                }
            }
        }
    }
}