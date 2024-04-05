using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using jungmin;

namespace Jc
{
    public class DropItem : PooledObject
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private global::ItemData itemData;

        [SerializeField]
        private Rigidbody rigid;
        public Rigidbody Rigid { get { return rigid; } }

        [SerializeField]
        private Transform obTr;
        [SerializeField]
        private Collider ownerCol;
        [SerializeField]
        private Collider obCol;

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        private bool autoRotate = false;
        [SerializeField]
        private float rotSpeed;
        [SerializeField]
        private float getSpeed;

        private Coroutine getItemRoutine;

        private void OnEnable()
        {
            InitSetting();
        }

        private void Update()
        {
            // 아이템 회전 적용
            if (autoRotate)
                obTr.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }

        private void InitSetting()
        {
            ownerCol.enabled = false;
            obCol.enabled = true;
            autoRotate = false;
            rigid.useGravity = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!Manager.Layer.groundLM.Contain(collision.gameObject.layer)) 
                return;
            
            rigid.velocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;
            autoRotate = true;
            obCol.enabled = false;
            ownerCol.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Player player = other.gameObject.GetComponent<PlayerTrigger>().owner;
                if (player == null) return;
                getItemRoutine = StartCoroutine(GetItemRoutine(player));
            }
        }

        // 베지어 곡선을 활용한 아이템 습득효과
        IEnumerator GetItemRoutine(Player player)
        {
            Vector3[] points = new Vector3[3];
            // 아이템 위치와 플레이어 사이의 1/5 지점 뒤를 경유할 포인트로 지정
            Vector3 vec = player.transform.position - transform.position;
            float dist = vec.magnitude / 8f;
            points[0] = transform.position -vec.normalized * dist;

            float time = 0f;
            yield return null;

            while(time < 0.95f)
            {
                // 변경값 1
                points[1] = Vector3.Lerp(transform.position, points[0], time);
                // 변경값 2
                points[2] = Vector3.Lerp(points[0], player.transform.position, time);
                transform.position = Vector3.Lerp(points[1], points[2], time);
                time += Time.deltaTime / getSpeed;
                yield return null;
            }

            gameObject.SetActive(false);
            transform.position = player.transform.position;
            player.GetItem(itemData);
            yield return null;
        }
    }
}
