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
            // �»�� ���� 8���� ����
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
        // �� size ������ ��ֹ��� 9ĭ �� count ������ŭ ��� �ʿ� ���� 
        public void SpawnObstacle(Obstacle obstacle, int size, int count)
        {
            // ������ ������ �� 20x20
            List<int> areaList = Enumerable.Range(1, 400).ToList();

            while (size > 0)
            {
                int num = Random.Range(1, areaList.Count);
                int z = num / 20 * 3;   // �� �̱�
                int x = (z == 0 ? num % 20 : num % 20 + 1) * 3;   // �� �̱�
                areaList.Remove(num);

                List<int> spawnableIDX = new List<int>();

                for(int i =0; i<spawnDirections.Count; i++)
                {
                    int nz = z + spawnDirections[i].z;
                    int nx = x + spawnDirections[i].x;

                    if (nz < 0 || nz >= Manager.Navi.mapZsize || nx < 0 || nx >= Manager.Navi.mapXsize) 
                        continue;
                    GroundType nType = Manager.Navi.gameMap[nz].groundList[nx].type;
                    // ��ֹ��� ��ġ�� �� �ִ� ���Ǹ� ����Ʈ�� ���
                    if (nType == GroundType.Empty || nType == GroundType.Buildable)
                        spawnableIDX.Add(i);
                }

                int cnt = count;
                while(cnt > 0 && spawnableIDX.Count > 0)
                {
                    // �������� �� ��ġ�� �̾� �ش� ��ġ�� ����
                    int rand = UnityEngine.Random.Range(0, spawnableIDX.Count - 1);
                    Ground getGround = 
                        Manager.Navi.gameMap[z + spawnDirections[spawnableIDX[rand]].z].groundList[x + spawnDirections[spawnableIDX[rand]].x];
                    Manager.Pool.GetPool(obstacle, getGround.transform.position, getGround.transform.rotation);
                    // �׶��� Ÿ�� ����
                    getGround.type = GroundType.Object;
                    spawnableIDX.RemoveAt(rand);

                    cnt--;
                    size--;
                }
            }
        }
    }
}
