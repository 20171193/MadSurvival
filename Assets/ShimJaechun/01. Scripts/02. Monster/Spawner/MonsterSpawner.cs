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

        [SerializeField]
        private GroundPos[] spawnablePos;
        public struct MapThreshold
        {
            public GroundPos min;
            public GroundPos max;
            public MapThreshold(GroundPos min, GroundPos max)
            {
                this.min = min;
                this.max = max;
            }
        }
        private MapThreshold[] mapThresholds;

        [SerializeField]
        private int spawnCount;

        public UnityAction OnAllMonsterDie;

        private void Awake()
        {
            //OnAllMonsterDie += gameFlow.ExitNight;
            // ������ �� �ִ� Ÿ���� ã������ bfs Ž���� ������ ������ ����
            // �� ũ�Ⱑ ����� ��� ����
            spawnablePos = new GroundPos[8] 
            {
                new GroundPos(0, 0), new GroundPos(0, 19), new GroundPos(0, 59),
                new GroundPos(19, 0),new GroundPos(19, 59),
                new GroundPos(59, 0),new GroundPos(59, 19),new GroundPos(59, 59)
            };
            mapThresholds = new MapThreshold[8]
            {
                new MapThreshold(new GroundPos(0,0), new GroundPos(19,19)),  new MapThreshold(new GroundPos(0,19), new GroundPos(19,39)), new MapThreshold(new GroundPos(0,39), new GroundPos(19,59)),
                new MapThreshold(new GroundPos(19,0), new GroundPos(39, 19)), new MapThreshold(new GroundPos(19,39), new GroundPos(39,59)),
                new MapThreshold(new GroundPos(39, 0), new GroundPos(59, 19)), new MapThreshold(new GroundPos(39, 19), new GroundPos(59,39)),new MapThreshold(new GroundPos(39, 39), new GroundPos(59,59))
            };
        }

        public void OnSpawn(int day)
        {
            if (spawnRoutine != null)
                StopCoroutine(spawnRoutine);

            if (!Manager.Data.daysWaveDataDic.ContainsKey(day))
                return;

            spawnRoutine = StartCoroutine(SpawnRoutine(day));
        }
        
        IEnumerator SpawnRoutine(int day)
        {
            int spawnWave = 0;
            yield return null;

            while(spawnWave <= 2)
            {
                Spawn(day, spawnWave++);
                yield return new WaitForSeconds(waveSpawnTime);
            }
        }
        private void Spawn(int day, int wave)
        {
            if (!Manager.Data.daysWaveDataDic.ContainsKey(day)) return;
            if (Manager.Data.daysWaveDataDic[day] == null) return;

            for(int i =0; i< Manager.Data.daysWaveDataDic[day].spawnList[wave].monsterList.Count; i++)
            {
                if (Manager.Data.daysWaveDataDic[day].spawnList[wave].monsterList[i] < 1) continue;
                string monsterName = Define.TryGetMonsterName((Define.MonsterName)i);

                for (int j = 0; j < Manager.Data.daysWaveDataDic[day].spawnList[wave].monsterList[i]; j++)
                {
                    Ground spawnGround = null;
                    while (spawnGround == null)
                    {
                        spawnGround = SetSpawnPos();
                    }

                    Monster spawned = (Monster)Manager.Pool.GetPool(Manager.Data.monsterDic[monsterName], spawnGround.transform.position, Quaternion.identity);
                    spawned.OnMonsterDie += MonsterDie;
                    spawned.FSM.ChangeState("Idle");
                    spawnCount++;
                }
            }
        }
        private Ground SetSpawnPos()
        {
            // 1. �� ���� 8�� ������ �̰� �������� �� ������ ����
            //   �»�, �߻�, ���
            //   ����,     , ����
            //   ����, ����, ����
            // 2. ������ ������ �߾Ӻ��� bfs Ž���� �ϸ� ����ִ� Ÿ���� ã��
            // 3. ����ִ� Ÿ���� ������ Ÿ�Ϸ� ����
            int rand = UnityEngine.Random.Range(0, 7);
            GroundPos startPos = spawnablePos[rand];

            // ����ִ� Ÿ���̶�� �ٷ� ����
            if (Manager.Navi.gameMap[startPos.z].groundList[startPos.x].type == GroundType.Empty)
                return Manager.Navi.gameMap[startPos.z].groundList[startPos.x];

            // bfs Ž��
            Queue<GroundPos> q = new Queue<GroundPos>();
            bool[,] visited = new bool[21, 21];

            // �������� �� �������� �Ÿ�
            int resol = Manager.Navi.mapZsize / 3 / 2;

            // Ž������ ����
            int minZ = mapThresholds[rand].min.z;
            int maxZ = mapThresholds[rand].max.z;
            int minX = mapThresholds[rand].min.x;
            int maxX = mapThresholds[rand].max.x;

            // Ž������ ����
            int[] dz = new int[4] { 0, 0, 1, -1 };
            int[] dx = new int[4] { 1, -1, 0, 0 };

            q.Enqueue(startPos);
            visited[startPos.z - minZ, startPos.x - minX] = true;

            while (q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dz[i] + curPos.x;
                    if (nz < minZ || nz > maxZ || nx < minX || nx > maxX) continue;
                    if (visited[nz-minZ, nx-minX]) continue;

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
                GameFlowController.Inst.FadeControll(true);
        }
    }
}
