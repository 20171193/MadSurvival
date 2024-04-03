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
        private Item itemData;

        [SerializeField]
        private bool autoRotate;
        [SerializeField]
        private float rotSpeed;

        [SerializeField]
        private float getSpeed;

        [SerializeField]
        private Transform obTr;

        private Coroutine getItemRoutine;

        private void Update()
        {
            // 아이템 회전 적용
            if (autoRotate)
                obTr.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }

        public void OnDropItem()
        {

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
            // 아이템 위치와 플레이어 사이의 1/10 지점을 경유할 포인트로 지정
            Vector3 vec = player.transform.position - transform.position;
            float dist = vec.magnitude / 8f;
            points[0] = transform.position + -vec.normalized * dist;

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
