using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] Transform _StartPoint;    // 이동 시작 지점
    [SerializeField] Transform _EndPoint;      // 이동 종료 지점
    [SerializeField] Transform _FollowTarget;  // 실제 카메라가 따라갈 대상

    [Header("회전")]
    [SerializeField] JoystickController _Joystick; // 조이스틱 입력
    [SerializeField] float _RotationSpeed = 100f;  // 회전 속도
    [SerializeField] float _MinY = -60f, _MaxY = 60f; // 회전 범위(Y축)

    GameSceneManager _GameSceneManager;  // 씬 매니저 참조

    float _Timer;         // 이동 시간 누적용 타이머
    bool _CanRotate;      // 이동 완료 후 회전 가능 플래그
    float _CurrentYaw;    // 현재 Y축 회전 각도

    private void Start()
    {
        // 1) 씬 매니저 가져오기
        _GameSceneManager = GameObject
            .FindGameObjectWithTag("GameSceneManager")
            .GetComponent<GameSceneManager>();

        // 2) 카메라 위치·회전 초기화
        _FollowTarget.position = _StartPoint.position;
        _FollowTarget.rotation = _StartPoint.rotation;
        _CurrentYaw = _FollowTarget.eulerAngles.y;
    }

    private void Update()
    {
        // 3) 오프닝 이동 중 처리
        if (!_CanRotate)
        {
            // 이동 진행률 계산
            float t = Mathf.Clamp01((_Timer += Time.deltaTime) / _GameSceneManager.GetOpeningDuration);
            // 위치/회전 보간
            _FollowTarget.position = Vector3.Lerp(_StartPoint.position, _EndPoint.position, t);
            _FollowTarget.rotation = Quaternion.Slerp(_StartPoint.rotation, _EndPoint.rotation, t);

            // 이동 완료 시 회전 허용
            if (t >= 1f) _CanRotate = true;
            return;
        }

        // 4) 이동 완료 후 조이스틱으로 회전 제어
        Vector2 tDir = _Joystick._InputDirection;  
        float tDeltaYaw = -tDir.x * _RotationSpeed * Time.deltaTime;
        _CurrentYaw = Mathf.Clamp(_CurrentYaw + tDeltaYaw, _MinY, _MaxY);

        // Y축 회전 적용
        var e = _FollowTarget.eulerAngles;
        _FollowTarget.eulerAngles = new Vector3(e.x, _CurrentYaw, e.z);
    }
}
