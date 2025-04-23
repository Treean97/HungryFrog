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
    RectTransform _ControllerBackground; // ���̽�ƽ ���
    [SerializeField]
    RectTransform _ControllerHandle; // ���̽�ƽ �ڵ�

    private Vector2 _InputDirection;

    [SerializeField]
    CinemachineFreeLook _CineFreeLookCamera;

    bool _IsDragging = false;

    private void Start()
    {
        // �ʱ� �ڵ� ��ġ ����
        _ControllerHandle.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if(_IsDragging)
        {
            // X �Է°�
            float tXDirection = _InputDirection.x;

            _CineFreeLookCamera.m_XAxis.Value += tXDirection * _RotationSpeed * Time.deltaTime;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� ����
        _IsDragging = true;

        // �巡���� ��ġ ���
        Vector2 tPosition = eventData.position - (Vector2)_ControllerBackground.position;
        _InputDirection = tPosition.normalized;

        // �ڵ��� ��ġ�� ����
        float _HandleDistance = Mathf.Clamp(tPosition.magnitude, 0, _ControllerBackground.sizeDelta.x / 2);
        _ControllerHandle.anchoredPosition = _InputDirection * _HandleDistance;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // �巡�� ����
    }    

    public void OnPointerUp(PointerEventData eventData)
    {
        // �巡�� ����
        _IsDragging = false;

        // ���̽�ƽ �ڵ��� �߾����� �ǵ���
        _InputDirection = Vector2.zero;
        _ControllerHandle.anchoredPosition = Vector2.zero;
    }

}
