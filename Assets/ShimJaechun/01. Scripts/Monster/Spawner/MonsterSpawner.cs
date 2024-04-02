using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameFlowController gameFlow;

        [SerializeField]
        private float waveSpawnTime;

        private Coroutine spawnRoutine;

        private Dictionary<int, WaveData> waveDataDic;

        [SerializeField]
        private GroundPos[] spawnablePos;

        [SerializeField]
        private int spawnCount;

        public UnityAction OnAllMonsterDie;

        private void Awake()
        {
            //OnAllMonsterDie += gameFlow.ExitNight;
            // 스폰할 수 있는 타일을 찾기위해 bfs 탐색을 시작할 영역들 모음
            // 맵 크기가 변경될 경우 수정
            spawnablePos = new GroundPos[8] 
            {
                new GroundPos(0, 0), new GroundPos(0, 19), new GroundPos(0, 59),
                new GroundPos(19, 0),new GroundPos(19, 59),
                new GroundPos(59, 0),new GroundPos(59, 19),new GroundPos(59, 59)
            };
        }

        public void OnSpawn(int day)
        {
            if (spawnRoutine != null)
                StopCoroutine(spawnRoutine);

            waveDataDic = Manager.Data.daysWaveDataDic[day];
            spawnRoutine = StartCoroutine(SpawnRoutine());
        }
        
        IEnumerator SpawnRoutine()
        {
            int spawnWave = 1;
            yield return null;

            while(spawnWave <= 3)
            {
                Spawn(spawnWave++);
                yield return new WaitForSeconds(waveSpawnTime);
            }
        }

        private void Spawn(int wave)
        {
            if (waveDataDic == null) return;
            if (!waveDataDic.ContainsKey(wave)) return;

            foreach(SpawnInfo spawnInfo in waveDataDic[wave].spawnList)
            {
                string monsterName = spawnInfo.monsterName;
                for(int i =0; i<spawnInfo.count; i++)
                {
                    Ground spawnGround = null;
                    while(spawnGround == null)
                    {
                        spawnGround = SetSpawnPos();
                    }
                    Manager.Pool.GetPool(Manager.Data.monsterDic[monsterName], spawnGround.transform.position, Quaternion.identity);
                    spawnCount++;
                }
            }
        }

        private Ground SetSpawnPos()
        {
            // 1. 맵 기준 8개 영역을 뽑고 랜덤으로 한 영역을 지정
            //   좌상, 중상, 우상
            //   좌중,     , 우중
            //   좌하, 중하, 우하
            // 2. 지정된 영역의 중앙부터 bfs 탐색을 하며 비어있는 타일을 찾기
            // 3. 비어있는 타일을 스폰할 타일로 지정

            GroundPos startPos = spawnablePos[UnityEngine.Random.Range(0, 7)];

            // 비어있는 타일이라면 바로 스폰
            if (Manager.Navi.gameMap[startPos.z].groundList[startPos.x].type == GroundType.Empty)
                return Manager.Navi.gameMap[startPos.z].groundList[startPos.x];

            // bfs 탐색
            Queue<GroundPos> q = new Queue<GroundPos>();
            bool[,] visited = new bool[20, 20];

            // 중점부터 각 변까지의 거리
            int resol = Manager.Navi.mapZsize / 3 / 2;

            // 탐색범위 설정
            int minZ = startPos.z - resol+1;
            int maxZ = startPos.z + resol;
            int minX = startPos.x - resol + 1;
            int maxX = startPos.x + resol;

            // 탐색방향 설정
            int[] dz = new int[4] { 0, 0, 1, -1 };
            int[] dx = new int[4] { 1, -1, 0, 0 };

            q.Enqueue(startPos);
            visited[startPos.z - minZ, startPos.x - minX] = true;
            while(q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dz[i] + curPos.x;
                    if (nz < minZ || nz > maxZ || nx < minX || nx > maxX) continue;
                    if (visited[nz, nx]) continue;

                    if (Manager.Navi.gameMap[nz].groundList[nx].type == GroundType.Empty)
                        return Manager.Navi.gameMap[nz].groundList[nx];

                    visited[nz-minZ, nx-minX] = true;
                    q.Enqueue(new GroundPos(nz, nx));
                }
            }
            return null;
        }

        public void MonsterDie(Monster monster)
        {
            monster.OnMonsterDie -= MonsterDie;

            spawnCount--;

            if (spawnCount <= 0)
                OnAllMonsterDie?.Invoke();
        }
    }
}
