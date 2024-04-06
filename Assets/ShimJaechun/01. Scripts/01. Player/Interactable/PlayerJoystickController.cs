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

        // ���콺 �Է� �� ���̽�ƽ ��ġ ����
        private void OnMouseClick(InputValue value)
        {
            if (owner.IsOnBackpack) return;
            if (Input.mousePosition.y < 120F) return;


            if (value.isPressed)
            {
                Vector2 mousePos = Input.mousePosition / joystickCanvas.scaleFactor;
                // ȭ���� ������ �Ѿ ��� ����
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
            // �Է��� ���� ��� ����ó��
            if (joystick.MoveDir == Vector3.zero)
            {
                curSpeed = 0f;
                anim.SetFloat("LeverLength", 0);
                return;
            }

            float leverRatio = joystick.LeverDistance / joystick.LeverRange;
            anim.SetFloat("LeverLength", leverRatio);

            // ĳ���� ���� ���� ����
            Vector3 moveDir = new Vector3(joystick.MoveDir.x, 0, joystick.MoveDir.y);

            curSpeed = maxSpeed * leverRatio;
            // �̵��������� �ٷ� ȸ���ϱ����� ȸ�� �� ���� 
            transform.forward = moveDir;
            // ������ ��ġ�� ���� �̵��ӵ� ����
            controller.Move(transform.forward * curSpeed * Time.deltaTime);
        }
    }
}

