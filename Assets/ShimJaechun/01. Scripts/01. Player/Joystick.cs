using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace Jc
{
    public class Joystick : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private Image backGroundImage;
        [SerializeField]
        private Image leverImage;
        [SerializeField]
        private RectTransform lever;
        private RectTransform back;
        [SerializeField]
        private Vector2 originPos;
        [SerializeField]
        private OnScreenStick leverStick;
        public OnScreenStick LeverStick { get { return leverStick; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [Header("조이스틱 내부의 레버 이동범위 제한")]
        [SerializeField, Range(10, 150)]
        private float leverRange;
        public float LeverRange { get { return leverRange; } }

        [SerializeField]
        private float leverDistance;
        public float LeverDistance { get { return leverDistance; } }

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private Vector3 moveDir;
        public Vector3 MoveDir { get { return moveDir; } }

        private Vector2 sizeDelta;

        // 활성화 / 비활성화 상태체크
        bool isEnable = false;

        private void Awake()
        {
            back = GetComponent<RectTransform>();
            originPos = back.anchoredPosition;

            sizeDelta = new Vector2(back.sizeDelta.x / 2f, back.sizeDelta.y / 2f);
            leverStick.movementRange = leverRange;
        }

        private void Update()
        {
            if (isEnable)
                SetDirection();
        }

        public void SetDirection()
        {
            // 조이스틱 레버에 따른 이동 방향설정
            Vector3 vec = lever.anchoredPosition;
            moveDir = vec.normalized;
            leverDistance = vec.magnitude;
        }

        public void EnableJoystick(Vector2 mousePos)
        {
            isEnable = true;
            back.anchoredPosition = mousePos - sizeDelta;
        }
        public void DisableJoystick()
        {
            isEnable = false;

            // 초기상태 (비활성화 상태로 복구)
            back.anchoredPosition = originPos;
            lever.anchoredPosition = Vector2.zero;
            leverDistance = 0f;
            moveDir = Vector3.zero;
        }
    }
}