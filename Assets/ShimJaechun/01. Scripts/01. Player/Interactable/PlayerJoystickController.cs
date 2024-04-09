using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Jc
{
    public class PlayerJoystickController : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private CharacterController controller;
        public CharacterController Controller { get { return controller; } }
        [SerializeField]
        private Canvas joystickCanvas;
        [SerializeField]
        private Joystick joystick;

        [SerializeField]
        private Player owner;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private bool isRun;

        // 마우스 입력 시 조이스틱 위치 설정
        private void OnMouseClick(InputValue value)
        {
            if (owner.IsOnBackpack) return;
            if (Input.mousePosition.y < 120F) return;


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

        public void Move(float maxSpeed, ref float curSpeed, Animator anim)
        {
            // 입력이 없는 경우 예외처리
            if (joystick.MoveDir == Vector3.zero)
            {
                curSpeed = 0f;
                anim.SetFloat("LeverLength", 0);
                return;
            }

            float leverRatio = joystick.LeverDistance / joystick.LeverRange;
            anim.SetFloat("LeverLength", leverRatio);

            // 캐릭터 전면 방향 설정
            Vector3 moveDir = new Vector3(joystick.MoveDir.x, 0, joystick.MoveDir.y);

            curSpeed = maxSpeed * leverRatio;
            // 이동방향으로 바로 회전하기위한 회전 선 세팅 
            transform.forward = moveDir;
            // 레버의 위치에 따른 이동속도 적용
            controller.Move(transform.forward * curSpeed * Time.deltaTime);
        }
    }
}

