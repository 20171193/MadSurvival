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

        [SerializeField]
        private List<Animal> animalPrefabs;

        private List<Animal> spawnedAnimal;

        private void Awake()
        {
            spawnedAnimal = new List<Animal>();

            // 스폰할 수 있는 타일을 찾기위해 bfs 탐색을 시작할 영역들 모음
            // 맵 크기가 변경될 경우 수정
            spawnablePos = new GroundPos[8]
            {
                new GroundPos(9, 9), new GroundPos(9, 29), new GroundPos(9, 49),
                new GroundPos(19, 9),new GroundPos(19, 49),
                new GroundPos(49, 9),new GroundPos(49, 29),new GroundPos(49, 49)
            };
            mapThresholds = new MapThreshold[8]
            {
                new MapThreshold(new GroundPos(0,0), new GroundPos(19,19)),new MapThreshold(new GroundPos(0,19), new GroundPos(19,39)), new MapThreshold(new GroundPos(0,39), new GroundPos(19,59)),
                new MapThreshold(new GroundPos(19,0), new GroundPos(39, 19)),new MapThreshold(new GroundPos(19,40), new GroundPos(39,59)),
                new MapThreshold(new GroundPos(39, 0), new GroundPos(59, 19)),new MapThreshold(new GroundPos(39, 20), new GroundPos(59,39)),new MapThreshold(new GroundPos(39, 39), new GroundPos(59,59))
            };

            // 동물 풀링
            foreach(Animal animal in animalPrefabs)
                Manager.Pool.CreatePool(animal, animal.Size, 15);
        }

        // 모든 동물들을 비활성화
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
            // 16일 이후는 16일차의 동물을 계속해서 스폰
            if (day > 16) day = 16;

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
                        Debug.Log($"{info.animalName} 이 존재하지 않아 스폰이 불가합니다.");
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
            // 1. 맵 기준 8개 영역을 뽑고 랜덤으로 한 영역을 지정
            //   좌상, 중상, 우상
            //   좌중,     , 우중
            //   좌하, 중하, 우하
            // 2. 지정된 영역의 중앙부터 bfs 탐색을 하며 비어있는 타일을 찾기
            // 3. 비어있는 타일을 스폰할 타일로 지정
            int rand = UnityEngine.Random.Range(0, 7);
            GroundPos startPos = spawnablePos[rand];

            // 비어있는 타일이라면 바로 스폰
            if (Manager.Navi.gameMap[startPos.z].groundList[startPos.x].type == GroundType.Empty)
                return Manager.Navi.gameMap[startPos.z].groundList[startPos.x];

            // bfs 탐색
            Queue<GroundPos> q = new Queue<GroundPos>();
            bool[,] visited = new bool[21, 21];

            // 중점부터 각 변까지의 거리
            int resol = Manager.Navi.mapZsize / 3 / 2;

            // 탐색범위 설정
            int minZ = mapThresholds[rand].min.z;
            int maxZ = mapThresholds[rand].max.z;
            int minX = mapThresholds[rand].min.x;
            int maxX = mapThresholds[rand].max.x;

            // 탐색방향 설정
            int[] dz = new int[4] { 0, 0, 1, -1 };
            int[] dx = new int[4] { 1, -1, 0, 0 };

            q.Enqueue(startPos);
            Debug.Log($"스폰 : {startPos.z - minZ}, {startPos.x - minX}");
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
