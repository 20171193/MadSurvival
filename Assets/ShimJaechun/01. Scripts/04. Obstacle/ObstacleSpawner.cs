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

        public UnityAction OnEnterNextDay;

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

            // 로드 장애물 데이터
            Manager.Data.LoadData(DataType.ObstacleData);
            // 로드 장애물 스폰 데이터 
            Manager.Data.LoadData(DataType.DaysObstacleData);
        }

        public void OnDestroyObstacle()
        {
            
        }

        // 하루가 지나고 스폰
        public void SpawnObstacle(int day)
        {
            // 16일 이후는 16일차의 오브젝트를 스폰
            if (day > 16) day = 16;

            // 기존에 스폰되었던 장애물 제거
            OnEnterNextDay?.Invoke();

            DaysObstacleData daysObstacleData = Manager.Data.daysObstacleDataDic[day];
            for(int i=0; i<3; i++)
            {
                SpawnObstacle(tree, daysObstacleData.trees[i], 1, "Tree", i);
                SpawnObstacle(stone, daysObstacleData.stones[i], 1, "Stone", i);
            }
        }

        // 총 size 개수의 장애물을 9칸 당 count 개수만큼 모든 맵에 생성 
        public void SpawnObstacle(Obstacle obstacle, int size, int count, string name, int level)
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

                // 스폰할 수 있는 구역 선별
                for(int i =0; i<spawnDirections.Count; i++)
                {
                    int nz = z + spawnDirections[i].z;
                    int nx = x + spawnDirections[i].x;

                    // 맵 범위를 벗어난 경우 continue
                    if (nz < 0 || nz >= Manager.Navi.mapZsize || nx < 0 || nx >= Manager.Navi.mapXsize) 
                        continue;
                    
                    // 해당 타일의 현재 타입을 할당
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
                    Obstacle inst = (Obstacle)Manager.Pool.GetPool(obstacle, getGround.transform.position, getGround.transform.rotation);
                    // 레벨 지정
                    inst.InitSetting(name, level, this);
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
