using Jc;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Jc
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField]
        private Obstacle tree;

        [SerializeField]
        private Obstacle stone;

        [SerializeField]
        private List<GroundPos> spawnDirections;

        private void Awake()
        {
            spawnDirections = new List<GroundPos>();
            // 좌상단 기준 8방향 설정
            for (int z = 0; z < 3; z++)
            {
                for (int x = 0; x < 3; x++)
                {
                    GroundPos nPos = new GroundPos(z, x);
                    spawnDirections.Add(nPos);
                }
            }

            Manager.Pool.CreatePool(tree, tree.Size, tree.Size + 10);
            Manager.Pool.CreatePool(stone, stone.Size, stone.Size + 10);
        }
        public void InitSpawn()
        {
            SpawnObstacle(tree, tree.SpawnCount, 2);
            SpawnObstacle(stone, stone.SpawnCount, 1);
        }
        // 총 size 개수의 장애물을 9칸 당 count 개수만큼 모든 맵에 생성 
        public void SpawnObstacle(Obstacle obstacle, int size, int count)
        {
            // 구역의 개수는 총 20x20
            List<int> areaList = Enumerable.Range(1, 400).ToList();

            while (size > 0)
            {
                int num = Random.Range(1, areaList.Count);
                int z = num / 20 * 3;   // 열 뽑기
                int x = (z == 0 ? num % 20 : num % 20 + 1) * 3;   // 행 뽑기
                areaList.Remove(num);

                List<int> spawnableIDX = new List<int>();

                for(int i =0; i<spawnDirections.Count; i++)
                {
                    int nz = z + spawnDirections[i].z;
                    int nx = x + spawnDirections[i].x;

                    if (nz < 0 || nz >= Manager.Navi.mapZsize || nx < 0 || nx >= Manager.Navi.mapXsize) 
                        continue;
                    GroundType nType = Manager.Navi.gameMap[nz].groundList[nx].type;
                    // 장애물이 위치할 수 있는 조건만 리스트에 담기
                    if (nType == GroundType.Empty || nType == GroundType.Buildable)
                        spawnableIDX.Add(i);
                }

                int cnt = count;
                while(cnt > 0 && spawnableIDX.Count > 0)
                {
                    // 랜덤으로 한 위치를 뽑아 해당 위치에 스폰
                    int rand = UnityEngine.Random.Range(0, spawnableIDX.Count - 1);
                    Ground getGround = 
                        Manager.Navi.gameMap[z + spawnDirections[spawnableIDX[rand]].z].groundList[x + spawnDirections[spawnableIDX[rand]].x];
                    Manager.Pool.GetPool(obstacle, getGround.transform.position, getGround.transform.rotation);
                    // 그라운드 타입 변경
                    getGround.type = GroundType.Object;
                    spawnableIDX.RemoveAt(rand);

                    cnt--;
                    size--;
                }
            }
        }
    }
}
