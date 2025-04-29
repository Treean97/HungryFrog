//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Cinemachine;
//using UnityEngine.EventSystems;
//using Unity.VisualScripting;

//public class GameSceneController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
//{
//    [SerializeField]
//    float _RotationSpeed;

//    [SerializeField]
//    RectTransform _ControllerBackground; // 조이스틱 배경
//    [SerializeField]
//    RectTransform _ControllerHandle; // 조이스틱 핸들

//    private Vector2 _InputDirection;

//    [SerializeField]
//    CinemachineFreeLook _CineFreeLookCamera;

//    bool _IsDragging = false;

//    private void Start()
//    {
//        // 초기 핸들 위치 설정
//        _ControllerHandle.anchoredPosition = Vector2.zero;
//    }

//    private void Update()
//    {
//        if (_IsDragging)
//        {
//            // X 입력값
//            float tXDirection = _InputDirection.x;

//            _CineFreeLookCamera.m_XAxis.Value += tXDirection * _RotationSpeed * Time.deltaTime;
//        }
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        // 드래그 시작
//        _IsDragging = true;

//        // 드래그한 위치 계산
//        Vector2 tPosition = eventData.position - (Vector2)_ControllerBackground.position;
//        _InputDirection = tPosition.normalized;

//        // 핸들의 위치를 조정
//        float _HandleDistance = Mathf.Clamp(tPosition.magnitude, 0, _ControllerBackground.sizeDelta.x / 2);
//        _ControllerHandle.anchoredPosition = _InputDirection * _HandleDistance;

//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        OnDrag(eventData); // 드래그 시작
//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        // 드래그 종료
//        _IsDragging = false;

//        // 조이스틱 핸들을 중앙으로 되돌림
//        _InputDirection = Vector2.zero;
//        _ControllerHandle.anchoredPosition = Vector2.zero;
//    }

//}

//using UnityEngine;
//using UnityEngine.EventSystems;
//using Unity.Cinemachine;

//public class GameSceneController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
//{
//    [Header("카메라 설정")]
//    [SerializeField] private CinemachineFreeLook _FreeLookCam;
//    [SerializeField] private Transform _StartPoint;
//    [SerializeField] private Transform _EndPoint;
//    [SerializeField] private Transform _FollowTarget; // Follow/LookAt에 쓰는 실제 이동할 대상
//    [SerializeField] private float _MoveDuration = 2f;

//    [Header("조이스틱 설정")]
//    [SerializeField] private RectTransform _ControllerBackground;
//    [SerializeField] private RectTransform _ControllerHandle;
//    [SerializeField] private float _RotationSpeed = 1f;

//    private Vector2 _InputDirection;
//    private bool _IsDragging = false;
//    private bool _CanControlCamera = false;
//    private bool _IsMoving = true;

//    private float _Timer = 0f;
//    private Vector3 _StartPos;
//    private Quaternion _StartRot;

//    private void Start()
//    {
//        // Follow/LookAt 대상의 위치를 시작 위치로 옮긴다
//        _FollowTarget.position = _StartPoint.position;
//        _FollowTarget.rotation = _StartPoint.rotation;

//        _StartPos = _StartPoint.position;
//        _StartRot = _StartPoint.rotation;

//        _ControllerHandle.anchoredPosition = Vector2.zero;
//    }

//    private void Update()
//    {
//        if (_IsMoving)
//        {
//            _Timer += Time.deltaTime;
//            float t = Mathf.Clamp01(_Timer / _MoveDuration);

//            // Follow/LookAt 대상을 이동
//            _FollowTarget.position = Vector3.Lerp(_StartPos, _EndPoint.position, t);
//            _FollowTarget.rotation = Quaternion.Slerp(_StartRot, _EndPoint.rotation, t);

//            if (t >= 1f)
//            {
//                _IsMoving = false;
//                _CanControlCamera = true;
//            }
//        }

//        if (_IsDragging && _CanControlCamera)
//        {
//            float tXDirection = _InputDirection.x;
//            _FreeLookCam.m_XAxis.Value -= tXDirection * _RotationSpeed * Time.deltaTime;
//        }
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        _IsDragging = true;

//        Vector2 tPosition = eventData.position - (Vector2)_ControllerBackground.position;
//        _InputDirection = tPosition.normalized;

//        float handleDistance = Mathf.Clamp(tPosition.magnitude, 0, _ControllerBackground.sizeDelta.x / 2f);
//        _ControllerHandle.anchoredPosition = _InputDirection * handleDistance;
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        OnDrag(eventData);
//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        _IsDragging = false;
//        _InputDirection = Vector2.zero;
//        _ControllerHandle.anchoredPosition = Vector2.zero;
//    }
//}

using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Cinemachine;

public class GameSceneController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("카메라 설정")]
    [SerializeField] private CinemachineFreeLook _FreeLookCam;
    [SerializeField] private Transform _StartPoint;
    [SerializeField] private Transform _EndPoint;
    [SerializeField] private Transform _FollowTarget;
    [SerializeField] private float _MoveDuration = 2f;

    [Header("조이스틱 설정")]
    [SerializeField] private RectTransform _ControllerBackground;
    [SerializeField] private RectTransform _ControllerHandle;
    [SerializeField] private float _RotationSpeed = 1f;

    public static int JoystickTouchID = -1; // <- 발사 스크립트와 공유할 조이스틱 터치 ID

    private Vector2 _InputDirection;
    private bool _IsDragging = false;
    private bool _CanControlCamera = false;
    private bool _IsMoving = true;

    private float _Timer = 0f;
    private Vector3 _StartPos;
    private Quaternion _StartRot;

    private void Start()
    {
        _FollowTarget.position = _StartPoint.position;
        _FollowTarget.rotation = _StartPoint.rotation;

        _StartPos = _StartPoint.position;
        _StartRot = _StartPoint.rotation;

        _ControllerHandle.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if (_IsMoving)
        {
            _Timer += Time.deltaTime;
            float t = Mathf.Clamp01(_Timer / _MoveDuration);

            _FollowTarget.position = Vector3.Lerp(_StartPos, _EndPoint.position, t);
            _FollowTarget.rotation = Quaternion.Slerp(_StartRot, _EndPoint.rotation, t);

            if (t >= 1f)
            {
                _IsMoving = false;
                _CanControlCamera = true;
            }
        }

        if (_IsDragging && _CanControlCamera)
        {
            float tXDirection = _InputDirection.x;
            _FreeLookCam.m_XAxis.Value -= tXDirection * _RotationSpeed * Time.deltaTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _IsDragging = true;
        JoystickTouchID = eventData.pointerId; // 조이스틱 터치 ID 저장
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _IsDragging = false;
        _InputDirection = Vector2.zero;
        _ControllerHandle.anchoredPosition = Vector2.zero;
        JoystickTouchID = -1; // 터치 해제 시 초기화
    }

    public void OnDrag(PointerEventData eventData)
    {
        _IsDragging = true;

        Vector2 tPosition = eventData.position - (Vector2)_ControllerBackground.position;
        _InputDirection = tPosition.normalized;

        float handleDistance = Mathf.Clamp(tPosition.magnitude, 0, _ControllerBackground.sizeDelta.x / 2f);
        _ControllerHandle.anchoredPosition = _InputDirection * handleDistance;
    }
}
