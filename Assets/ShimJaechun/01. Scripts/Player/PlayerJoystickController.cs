using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jc
{
    public class PlayerJoystickController : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private GameObject[] models;
        private int curModel = 0;

        [SerializeField]
        private CharacterController controller;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private Canvas joystickCanvas;
        [SerializeField]
        private Joystick joystick;

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        private float maxWalkSpeed;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private bool isRun;

        private void Start()
        {
            SetModel(curModel);
        }

        private void Update()
        {
            Move();
        }
        private void SetModel(int index)
        {
            // 모델 변경함수
            // 예외처리 : 인덱스 범위를 벗어난 경우
            if (index >= models.Length || index < 0) return;

            // 현재 모델을 비활성화
            models[curModel]?.SetActive(false);
            
            // 모델 변경 후 활성화
            curModel = index;
            models[curModel].SetActive(true);
            // 애니메이터 변경
            anim = models[curModel].GetComponent<Animator>();            
        }

        // 테스트용 캐릭터 모델 변경
        private void OnCharacterChange(InputValue value)
        {
            SetModel(value.Get<float>() < 0 ? curModel-1 : curModel+1);
        }

        // 마우스 입력 시 조이스틱 위치 설정
        private void OnMouseClick(InputValue value)
        {
            if (value.isPressed)
            {
                Vector2 mousePos = Input.mousePosition / joystickCanvas.scaleFactor;
                // 화면의 절반을 넘어간 경우 리턴
                if (mousePos.x > Screen.width / 2) return;
                joystick.EnableJoystick(mousePos);
            }
            else
            {
                joystick.DisableJoystick();
            }
        }

        private void Move()
        {
            // 입력이 없는 경우 예외처리
            if (joystick.MoveDir == Vector3.zero)
            {
                anim.SetFloat("LeverLength", 0);
                return;
            }

            float leverRatio = joystick.LeverDistance / joystick.LeverRange;
            anim.SetFloat("LeverLength", leverRatio);

            // 캐릭터 전면 방향 설정
            Vector3 moveDir = new Vector3(joystick.MoveDir.x, 0, joystick.MoveDir.y);

            // 이동방향으로 바로 회전하기위한 회전 선 세팅 
            transform.forward = moveDir;
            controller.Move(transform.forward * maxWalkSpeed * leverRatio * Time.deltaTime);
        }
    }
}