using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("�̵�")]
    [SerializeField] Transform _StartPoint;
    [SerializeField] Transform _EndPoint;
    [SerializeField] Transform _FollowTarget;
    [SerializeField] float _MoveDuration = 2f;

    [Header("ȸ��")]
    [SerializeField] JoystickController _Joystick;
    [SerializeField] float _RotationSpeed = 100f;
    [SerializeField] float _MinY = -60f, _MaxY = 60f;

    float _Timer;
    bool _CanRotate;
    float _CurrentYaw;

    private void Start()
    {
        // �̵� �ʱ�ȭ
        _FollowTarget.position = _StartPoint.position;
        _FollowTarget.rotation = _StartPoint.rotation;
        _CurrentYaw = _FollowTarget.eulerAngles.y;
    }

    private void Update()
    {
        // 1) ���ۡ��ǥ �̵�
        if (!_CanRotate)
        {
            float t = Mathf.Clamp01((_Timer += Time.deltaTime) / _MoveDuration);
            _FollowTarget.position = Vector3.Lerp(_StartPoint.position, _EndPoint.position, t);
            _FollowTarget.rotation = Quaternion.Slerp(_StartPoint.rotation, _EndPoint.rotation, t);
            if (t >= 1f) _CanRotate = true;
            return;
        }

        // 2) �� ������ JoystickController.InputDirection �о ȸ��
        Vector2 tDir = _Joystick._InputDirection;
        float tDeltaYaw = tDir.x * _RotationSpeed * Time.deltaTime;
        _CurrentYaw = Mathf.Clamp(_CurrentYaw + tDeltaYaw, _MinY, _MaxY);

        var e = _FollowTarget.eulerAngles;
        _FollowTarget.eulerAngles = new Vector3(e.x, _CurrentYaw, e.z);
    }
}
