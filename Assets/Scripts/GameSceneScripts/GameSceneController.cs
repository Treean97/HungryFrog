using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GameSceneController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    float _RotationSpeed;

    [SerializeField]
    RectTransform _ControllerBackground; // 조이스틱 배경
    [SerializeField]
    RectTransform _ControllerHandle; // 조이스틱 핸들

    private Vector2 _InputDirection;

    [SerializeField]
    CinemachineFreeLook _CineFreeLookCamera;

    bool _IsDragging = false;

    private void Start()
    {
        // 초기 핸들 위치 설정
        _ControllerHandle.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if(_IsDragging)
        {
            // X 입력값
            float tXDirection = _InputDirection.x;

            _CineFreeLookCamera.m_XAxis.Value += tXDirection * _RotationSpeed * Time.deltaTime;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 시작
        _IsDragging = true;

        // 드래그한 위치 계산
        Vector2 tPosition = eventData.position - (Vector2)_ControllerBackground.position;
        _InputDirection = tPosition.normalized;

        // 핸들의 위치를 조정
        float _HandleDistance = Mathf.Clamp(tPosition.magnitude, 0, _ControllerBackground.sizeDelta.x / 2);
        _ControllerHandle.anchoredPosition = _InputDirection * _HandleDistance;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // 드래그 시작
    }    

    public void OnPointerUp(PointerEventData eventData)
    {
        // 드래그 종료
        _IsDragging = false;

        // 조이스틱 핸들을 중앙으로 되돌림
        _InputDirection = Vector2.zero;
        _ControllerHandle.anchoredPosition = Vector2.zero;
    }

}
