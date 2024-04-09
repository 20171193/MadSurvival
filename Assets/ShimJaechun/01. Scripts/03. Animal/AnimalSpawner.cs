using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Jc.MonsterSpawner;

namespace Jc
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField]
        private GroundPos[] spawnablePos;
        private MapThreshold[] mapThresholds;

        private List<Animal> spawnedAnimal;

        private void Awake()
        {
            spawnedAnimal = new List<Animal>();

            // ������ �� �ִ� Ÿ���� ã������ bfs Ž���� ������ ������ ����
            // �� ũ�Ⱑ ����� ��� ����
            spawnablePos = new GroundPos[8]
            {
                new GroundPos(9, 9), new GroundPos(9, 29), new GroundPos(9, 49),
                new GroundPos(19, 9),new GroundPos(19, 49),
                new GroundPos(49, 9),new GroundPos(49, 29),new GroundPos(49, 49)
            };
            mapThresholds = new MapThreshold[8]
            {
                new MapThreshold(new GroundPos(0,0), new GroundPos(19,19)),new MapThreshold(new GroundPos(0,20), new GroundPos(19,39)), new MapThreshold(new GroundPos(0,40), new GroundPos(19,59)),
                new MapThreshold(new GroundPos(20,0), new GroundPos(39, 19)),new MapThreshold(new GroundPos(20,40), new GroundPos(39,59)),
                new MapThreshold(new GroundPos(40, 0), new GroundPos(59, 19)),new MapThreshold(new GroundPos(40, 20), new GroundPos(59,39)),new MapThreshold(new GroundPos(40, 40), new GroundPos(59,59))
            };

        }

        // ��� �������� ��Ȱ��ȭ
        public void ReturnAllAnimal()
        {
            if (spawnedAnimal.Count < 1) return;

            foreach(Animal animal in spawnedAnimal)
            {
                string state = animal.FSM.FSM.CurState;
                if (state == "Pooled" || state == "Die" || state == "ReturnPool") continue;

                animal.FSM.ChangeState("ReturnPool");
            }

            spawnedAnimal.Clear();
        }

        public void OnSpawn(int day)
        {
            if (!Manager.Data.daysAnimalDataDic.ContainsKey(day)) 
                return;

            if (Manager.Data.daysAnimalDataDic[day].Count < 1) 
                return;

            foreach (DaysAnimalInfo info in Manager.Data.daysAnimalDataDic[day])
            {
                for(int i =0; i<info.spawnCount; i++)
                {
                    if(!Manager.Data.animalDic.ContainsKey(info.animalName))
                    {
                        Debug.Log($"{info.animalName} �� �������� �ʾ� ������ �Ұ��մϴ�.");
                        continue;
                    }

                    Ground spawnGround = null;
                    while (spawnGround == null)
                    {
                        spawnGround = SetSpawnPos();
                    }

                    Animal inst = (Animal)Manager.Pool.GetPool(Manager.Data.animalDic[info.animalName], spawnGround.transform.position, Quaternion.identity);
                    spawnedAnimal.Add(inst);
                    inst.FSM.ChangeState("Idle");
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
            bool[,] visited = new bool[20, 20];

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
            Debug.Log($"���� : {startPos.z - minZ}, {startPos.x - minX}");
            visited[startPos.z - minZ, startPos.x - minX] = true;

            while (q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dz[i] + curPos.x;
                    if (nz < minZ || nz > maxZ || nx < minX || nx > maxX) continue;
                    if (visited[nz - minZ, nx - minX]) continue;

                    if (Manager.Navi.gameMap[nz].groundList[nx].type == GroundType.Empty)
                        return Manager.Navi.gameMap[nz].groundList[nx];

                    visited[nz - minZ, nx - minX] = true;
                    q.Enqueue(new GroundPos(nz, nx));
                }
            }
            return null;
        }

    }
}
