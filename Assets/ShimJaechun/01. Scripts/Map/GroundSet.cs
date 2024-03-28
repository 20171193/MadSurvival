using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct GroundList
    {
        [SerializeField]
        public List<Ground> groundList;

        public GroundList(List<Ground> groundList)
        {
            this.groundList = groundList;
        }
    }

    public class GroundSet : MonoBehaviour
    {
        [Header("Component")]
        [Space(2)]
        [SerializeField]
        private GameObject groundPrefab;

        [SerializeField]
        private MeshRenderer dummyPlaneMr;

        //[SerializeField]
        //private List<List<Ground>> groundList = new List<List<Ground>>();

        [SerializeField]
        private List<GroundList> groundLists;
        public List<GroundList> GroundLists { get { return groundLists; } } 

        // ������ ũ��
        [SerializeField]
        private float prefabXscale;
        [SerializeField]
        private float prefabZscale;

        // x�࿡ ������ ������ ���� 
        private int xCount;
        // z�࿡ ������ ������ ����
        private int zCount;

        // x �� ���� (ù �������� ������ x��ǥ)
        private float xStartPos;
        // z �� ���� (ù �������� ������ z��ǥ)
        private float zStartPos;

        private void Start()
        {
            // ��ã�� �Ŵ����� ���� ���� �Ҵ�
            //if (groundLists.Count > 0)
                //Manager.Navigation.AssginGameMap(groundLists);
        }


        [ContextMenu("DrawGround")]
        public void DrawGround()
        {
            groundLists.Clear();

            dummyPlaneMr.enabled = false;

            prefabXscale = groundPrefab.transform.localScale.x;
            prefabZscale = groundPrefab.transform.localScale.z;

            // ���� ���� ���� (���� ����)
            // ������ �������� 1 �Ǵ� 2�϶��� ���������� ����
            xStartPos = 4.5f * transform.localScale.x - 0.5f * (prefabXscale - 1);
            zStartPos = 4.5f * transform.localScale.z - 0.5f * (prefabZscale - 1);

            xCount = (int)(transform.localScale.x * 10 / prefabXscale);
            zCount = (int)(transform.localScale.z * 10 / prefabZscale);

            for (int z = 0; z < zCount; z++)
            {
                GroundList grounds = new GroundList(new List<Ground>());

                for (int x = 0; x < xCount; x++)
                {
                    Vector3 groundPos = new Vector3(
                        transform.position.x - xStartPos + x * prefabXscale,
                        transform.position.y,
                        transform.position.z - zStartPos + z * prefabZscale);
                    GameObject inst = Instantiate(groundPrefab, groundPos, Quaternion.identity);
                    grounds.groundList.Add(inst.GetComponent<Ground>());
                    inst.transform.parent = transform;
                    inst.gameObject.name = $"{x}{z}_Ground";
                }

                groundLists.Add(grounds);
            }
        }

        [ContextMenu("ResetGround")]
        public void ResetGround()
        {
            groundLists.Clear();
        }
    }
}
