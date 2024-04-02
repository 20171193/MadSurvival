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
        private float waveSpawnTime;

        public UnityAction OnEndSpawn;

        private Coroutine spawnRoutine;

        private Dictionary<int, WaveData> waveDataDic;

        [SerializeField]
        private GroundPos[] spawnablePos;

        private void Awake()
        {
            // ������ �� �ִ� Ÿ���� ã������ bfs Ž���� ������ ������ ����
            int midPos = Manager.Navi.mapXsize / 3 / 2 - 1;
            spawnablePos = new GroundPos[8] 
            {
                new GroundPos(midPos, midPos), new GroundPos(midPos, midPos*2), new GroundPos(midPos,midPos*3),
                new GroundPos(midPos*2, midPos),new GroundPos(midPos*2, midPos*3),
                new GroundPos(midPos*3, midPos),new GroundPos(midPos*3, midPos*2),new GroundPos(midPos*3, midPos*3)
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
                Monster monster = spawnInfo.monster;
                for(int i =0; i<spawnInfo.count; i++)
                {
                    Ground spawnGround = null;
                    while(spawnGround == null)
                    {
                        spawnGround = SetSpawnPos();
                    }
                    Manager.Pool.GetPool(monster, spawnGround.transform.position, Quaternion.identity);
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

            GroundPos startPos = spawnablePos[UnityEngine.Random.Range(0, 7)];

            // ����ִ� Ÿ���̶�� �ٷ� ����
            if (Manager.Navi.gameMap[startPos.z].groundList[startPos.x].type == GroundType.Empty)
                return Manager.Navi.gameMap[startPos.z].groundList[startPos.x];

            // bfs Ž��
            Queue<GroundPos> q = new Queue<GroundPos>();
            bool[,] visited = new bool[20, 20];

            // �������� �� �������� �Ÿ�
            int resol = Manager.Navi.mapZsize / 3 / 2;

            // Ž������ ����
            int minZ = startPos.z - resol+1;
            int maxZ = startPos.z + resol;
            int minX = startPos.x - resol + 1;
            int maxX = startPos.x + resol;

            // Ž������ ����
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
    }
}
