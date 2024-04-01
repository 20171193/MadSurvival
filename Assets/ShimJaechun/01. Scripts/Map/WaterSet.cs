using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class WaterSet : MonoBehaviour
    {
        [Serializable]
        public struct WaterGround
        {
            public GroundPos startPos;
            public int zSize;
            public int xSize;
            public WaterGround(GroundPos startPos, int zSize, int xSize)
            {
                this.startPos = startPos;
                this.zSize = zSize;
                this.xSize = xSize;
            }
        }

        [SerializeField]
        private GameObject waterPrefab;

        [SerializeField]
        private int waterSize;
        [SerializeField]
        private bool isRandomSpawn;
        [SerializeField]
        private int spawnSize;

        [Header("8���� ������ġ {0~3:�𼭸�, 4:����, 5:�Ʒ���, 6:������, 7:������}")]
        [SerializeField]
        private List<WaterGround> groundList;

        private void Awake()
        {
            groundList = new List<WaterGround>();
        }

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            // �������� spawnSize ��ŭ�� �̾Ƽ� ����
            if(isRandomSpawn)
            {
                List<int> idxList = new List<int>();
                for(int i = 0; i< groundList.Count; i++)
                {
                    idxList.Add(i);
                }

                int cnt = spawnSize;
                while (cnt > 0)
                {
                    int rand = UnityEngine.Random.Range(0, idxList.Count - 1);
                    SpawnWaterGround(idxList[rand]);
                    idxList.RemoveAt(rand);
                    cnt--;
                }
            }
            else
            {
                for(int i =0; i<groundList.Count; i++)
                {
                    SpawnWaterGround(i);
                }
            }
        }

        public void SpawnWaterGround(int index)
        {
            if (index < 0 || index >= groundList.Count)
            {
                Debug.Log("SpawnWaterGround : �ε��� ���� ���");
                return;
            }

            WaterGround waterGround = groundList[index];
            for (int z = waterGround.startPos.z; z < waterGround.startPos.z + waterGround.zSize; z++)
            {
                for (int x = waterGround.startPos.x; x < waterGround.startPos.x + waterGround.xSize; x++)
                {
                    Ground ground = Manager.Navi.gameMap[z].groundList[x];
                    Destroy(ground.transform.GetChild(0).gameObject);
                    GameObject water = Instantiate(waterPrefab, ground.transform.position + Vector3.up, Quaternion.identity);
                    water.transform.parent = transform;
                    water.name = $"{z},{x}_Water";
                    ground.type = GroundType.Water;
                }
            }
        }
        public void SettingGroundSize()
        {
            // �𼭸� ����
            int zSize = 8, xSize = 8;
            // 1. �»��
            groundList.Add(new WaterGround(new GroundPos(0, 0), zSize, xSize));
            // 2. ����
            groundList.Add(new WaterGround(new GroundPos(0, Manager.Navi.mapXsize - xSize), zSize, xSize));
            // 3. ���ϴ�
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize - zSize, 0), zSize, xSize));
            // 4. ���ϴ�
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize - zSize, Manager.Navi.mapXsize - xSize), zSize, xSize));

            // �߻�/���ϴ� ����
            zSize = 5;
            xSize = 13;
            // 1. �߻��
            groundList.Add(new WaterGround(new GroundPos(0, Manager.Navi.mapXsize / 2 - xSize / 2), zSize, xSize));
            // 2. ���ϴ�
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize - zSize, Manager.Navi.mapXsize / 2 - xSize / 2), zSize, xSize));

            // ����/���ߴ� ����
            zSize = 13;
            xSize = 5;
            // 1. ���ߴ�
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize / 2 - zSize / 2, 0), zSize, xSize));
            // 2. ���ߴ�
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize / 2 - zSize / 2, Manager.Navi.mapXsize - xSize), zSize, xSize));
        }
    }
}
