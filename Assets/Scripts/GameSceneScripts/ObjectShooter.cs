using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectShooter : MonoBehaviour
{
    [Header("Shooter 설정")]
    public Transform _ShooterTF;               // 발사 위치 및 방향
    public float _ShootForce = 0;              // 누적 발사력
    public float _PowerScale;                  // 발사력 증가 속도
    public float _MaxShootForce = 15f;         // 최대 발사력
    public float _MinShootForce = 5f;          // 최소 발사력
    public float _UpwardForce = 5f;            // 위쪽 추가 힘
    public float _MaxTorqueForce;              // 최대 회전 토크
    public float _MinTorqueForce;              // 최소 회전 토크

    [SerializeField] private AdditionalForceManager _AdditionalForceManager; // 추가 힘 매니저
    [SerializeField] private float _MaxShootDelay;    // 발사 지연 시간
    [SerializeField] private ObjectShootChance _ObjectShootChance;  // 발사 확률 정보
    [SerializeField] private GameSceneNextObjectsUI _NextObjectsUI; // 다음 오브젝트 UI

    private float _CurShootDelay;            // 현재 지연 누적 시간
    private bool _CanShoot = false;          // 발사 가능 플래그
    private bool _IsPressed = false;         // 입력 중 플래그
    public bool _IsCharging => _IsPressed && _CanShoot; // 충전 중 여부

    private bool _IsReleased = false;        // 입력 해제 플래그
    private Queue<ShootChanceInfo> _NextObjects = new Queue<ShootChanceInfo>(); // 다음 발사 오브젝트 큐

    private int _ShootTouchID = -1;           // 현재 발사용 터치 ID

    [SerializeField] GameSceneManager _GameSceneManager;       // 씬 관리 매니저
    [SerializeField] GameSceneUIManager _GameSceneUIManager;   // UI 매니저


    private void Start()
    {
        _ShootForce = 0;                                // 발사력 초기화
        _AdditionalForceManager.SetAdditionalForce();   // 첫 추가 힘 설정
        _CurShootDelay = _MaxShootDelay;                // 지연 초기화

        // 초기 큐 채우기 및 UI 갱신
        for (int i = 0; i < _NextObjectsUI._SlotImages.Count; i++)
            _NextObjects.Enqueue(GetRandomObjectInfo());
        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);
    }

    private void Update()
    {
        if (!CheckIsCanShoot()) return;  // 발사 가능 상태인지 확인

        // 지연 시간 누적
        if (_CurShootDelay < _MaxShootDelay)
            _CurShootDelay += Time.deltaTime;
        else
            _CanShoot = true;

        // 입력 상태 체크
        _IsPressed = Input.GetKey(KeyCode.Space) || IsTouchPressed();
        _IsReleased = Input.GetKeyUp(KeyCode.Space) || IsTouchReleased();

        if (_IsPressed && _CanShoot)
            SettingShootPower();   // 발사력 충전

        if (_IsReleased && _CanShoot)
        {
            ShootObject();                           // 오브젝트 발사
            _AdditionalForceManager.SetAdditionalForce(); // 다음 추가 힘 설정
        }
    }

    // 발사 가능 여부 판단
    bool CheckIsCanShoot()
    {
        if (_GameSceneManager.GetIsOpening) return false;       // 오프닝 중 차단
        if (UIBtnBase._IsBlocking) return false;                // 버튼 UI 차단
        if (_GameSceneUIManager.GetUIIsRunning) return false;   // 설정 UI 열림 차단
        return true;
    }

    // 터치 입력 시작 확인
    private bool IsTouchPressed()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.fingerId == JoystickController._TouchID) continue;
            if (_ShootTouchID == -1 && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                _ShootTouchID = touch.fingerId;
                return true;
            }
            if (touch.fingerId == _ShootTouchID && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
                return true;
        }
        return false;
    }

    // 터치 입력 해제 확인
    private bool IsTouchReleased()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.fingerId == _ShootTouchID && touch.phase == TouchPhase.Ended)
            {
                _ShootTouchID = -1;
                return true;
            }
        }
        return false;
    }

    // 발사력 증가 연산
    private void SettingShootPower()
    {
        _ShootForce += Time.deltaTime * _PowerScale;
    }

    // 실제 오브젝트 발사 처리
    private void ShootObject()
    {
        ShootChanceInfo tSelectedInfo = _NextObjects.Dequeue();  // 큐에서 선택
        GameObject tShootObjectGO = ObjectPoolManager._Inst.GetObject(tSelectedInfo.ShootObjectsData.GetShootObjectName.ToString());

        // 위치 및 리지드바디 참조
        tShootObjectGO.transform.position = _ShooterTF.position;
        Rigidbody tRig = tShootObjectGO.GetComponent<Rigidbody>();

        // 힘 계산 및 클램핑
        Vector3 tForce = _ShooterTF.forward * (_ShootForce + _MinShootForce)
                       + Vector3.up * _UpwardForce
                       + _AdditionalForceManager.GetAdditionalForceVector;
        float clampedMag = Mathf.Clamp(tForce.magnitude, _MinShootForce, _MaxShootForce);
        tForce = tForce.normalized * clampedMag;
        tRig.AddForce(tForce, ForceMode.Impulse);

        // 상태 초기화
        _ShootForce = 0;
        _CanShoot = false;
        _CurShootDelay = 0;

        // 랜덤 토크 적용
        Vector3 tRandomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * Random.Range(_MinTorqueForce, _MaxTorqueForce);
        tRig.AddTorque(tRandomTorque, ForceMode.Impulse);

        // 점수 추가
        GameSceneScoreManager._Inst.AddScoreByShoot(
            tShootObjectGO.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID);

        // 다음 큐 및 UI 갱신
        _NextObjects.Enqueue(GetRandomObjectInfo());
        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);

        // 발사 사운드
        ObjectShootSound();
    }

    // 확률에 따라 랜덤 오브젝트 정보 반환
    private ShootChanceInfo GetRandomObjectInfo()
    {
        float tRand = Random.Range(0f, 100f);
        float tSum = 0f;
        foreach (var tInfo in _ObjectShootChance._ShootChances)
        {
            tSum += tInfo.Chance;
            if (tRand <= tSum) return tInfo;
        }
        return _ObjectShootChance._ShootChances[^1];
    }

    // 발사 사운드 재생
    private void ObjectShootSound()
    {
        SoundManager._Inst.PlayRandomSFX(SoundCategory.ThrowObject);
    }
}
