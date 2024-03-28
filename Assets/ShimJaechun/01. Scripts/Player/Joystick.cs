using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

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
    private Vector2 originPos;
    [SerializeField]
    private OnScreenStick leverStick;

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

    // 활성화 / 비활성화 상태체크
    bool isEnable = false;

    private void Awake()
    {
        back = GetComponent<RectTransform>();
        originPos = back.anchoredPosition;

        leverStick.movementRange = leverRange;
    }

    private void Update()
    {
        if(isEnable)
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
        back.anchoredPosition = mousePos - originPos;
        // 이미지 알파값 +
        backGroundImage.color = new Color32(255,255,255, 150);
        leverImage.color = new Color32(255,255,255, 255);
    }
    public void DisableJoystick()
    {
        isEnable = false;

        // 초기상태 (비활성화 상태로 복구)
        back.anchoredPosition = originPos;
        lever.anchoredPosition = Vector2.zero;
        leverDistance = 0f;
        moveDir = Vector3.zero;

        // 이미지 알파값 -
        backGroundImage.color = new Color32(255,255,255, 80);
        leverImage.color = new Color32(255,255,255, 120);
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    //Debug.Log("이벤트 데이터 " + eventData.position);
    //    //var inputPos = eventData.position - rectTransform.anchoredPosition - originPos - originScale/2;
    //    // 조이스틱의 이동범위 제한
    //    //var inputVector = inputPos.magnitude < stickRange ? inputPos : inputPos.normalized * stickRange;
    //    //lever.anchoredPosition = inputVector;
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    var inputPos = eventData.position - rectTransform.anchoredPosition - originPos - originScale/2;
    //    // 조이스틱의 이동범위 제한
    //    var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
    //    lever.anchoredPosition = inputVector;
    //}
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    DisableJoystick();
    //}
}
