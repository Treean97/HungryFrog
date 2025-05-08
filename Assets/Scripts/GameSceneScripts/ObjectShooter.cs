using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectShooter : MonoBehaviour
{
    [Header("Shooter 설정")]
    public Transform _ShooterTF;
    public float _ShootForce = 0;
    public float _PowerScale;
    public float _MaxShootForce = 15f;
    public float _MinShootForce = 5f;
    public float _UpwardForce = 5f;
    public float _MaxTorqueForce;
    public float _MinTorqueForce;

    [SerializeField] private AdditionalForceManager _AdditionalForceManager;
    [SerializeField] private float _MaxShootDelay;
    //[SerializeField] private GameObject[] _ShootObjects;
    [SerializeField] private ObjectShootChance _ObjectShootChance;
    [SerializeField] private GameSceneNextObjectsUI _NextObjectsUI;

    private float _CurShootDelay;
    private bool _CanShoot = false;
    private bool _IsPressed = false;
    public bool _IsCharging => _IsPressed && _CanShoot;

    private bool _IsReleased = false;
    private Queue<ShootChanceInfo> _NextObjects = new Queue<ShootChanceInfo>();

    private int _ShootTouchID = -1; // 발사용 터치 ID


    [SerializeField]
    GameSceneManager _GameSceneManager;
    [SerializeField]
    GameSceneUIManager _GameSceneUIManager;



private void Start()
    {
        _ShootForce = 0;
        _AdditionalForceManager.SetAdditionalForce();
        _CurShootDelay = _MaxShootDelay;

        for (int i = 0; i < _NextObjectsUI._SlotImages.Count; i++)
        {
            _NextObjects.Enqueue(GetRandomObjectInfo());
        }

        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);

        
    }

    private void Update()
    {
        if(!CheckIsCanShoot())
        {
            return;
        }



        if (_CurShootDelay < _MaxShootDelay)
        {
            _CurShootDelay += Time.deltaTime;
        }
        else
        {
            _CanShoot = true;
        }

        _IsPressed = Input.GetKey(KeyCode.Space) || IsTouchPressed();
        _IsReleased = Input.GetKeyUp(KeyCode.Space) || IsTouchReleased();

        if (_IsPressed && _CanShoot)
        {
            SettingShootPower();
        }

        if (_IsReleased && _CanShoot)
        {
            ShootObject();
            _AdditionalForceManager.SetAdditionalForce();
        }
    }

    // 발사 불가능 조건
    bool CheckIsCanShoot()
    {
        // 게임 시작 후 일정 시간동안 발사 막기
        if (_GameSceneManager.GetIsOpening)
        {
            return false;
        }

        // 버튼 터치 시 발사 막기
        if (UIBtnBase._IsBlocking)
        {
            return false;
        }

        // UI 팝업 시 발사 막기
        if (_GameSceneUIManager.GetUIIsRunning)
        {
            return false;
        }

        return true;
    }

    private bool IsTouchPressed()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);

            if (touch.fingerId == JoystickController._TouchID)
                continue; // 조이스틱 터치는 무시

            if (_ShootTouchID == -1 && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                _ShootTouchID = touch.fingerId;
                return true;
            }

            if (touch.fingerId == _ShootTouchID && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                return true;
            }
        }

        return false;
    }

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


    private void SettingShootPower()
    {
        _ShootForce += Time.deltaTime * _PowerScale;
    }

    private void ShootObject()
    {
        ShootChanceInfo tSelectedInfo = _NextObjects.Dequeue();
        GameObject tShootObjectGO = ObjectPoolManager._Inst.GetObject(tSelectedInfo.ShootObjectsData.GetShootObjectName.ToString());

        tShootObjectGO.transform.position = _ShooterTF.position;
        Rigidbody tRig = tShootObjectGO.GetComponent<Rigidbody>();

        Vector3 tForce = _ShooterTF.forward * (_ShootForce + _MinShootForce)
                       + Vector3.up * _UpwardForce
                       + _AdditionalForceManager.GetAdditionalForceVector;

        float clampedMagnitude = Mathf.Clamp(tForce.magnitude, _MinShootForce, _MaxShootForce);
        tForce = tForce.normalized * clampedMagnitude;

        tRig.AddForce(tForce, ForceMode.Impulse);

        _ShootForce = 0;
        _CanShoot = false;
        _CurShootDelay = 0;

        Vector3 tRandomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * Random.Range(_MinTorqueForce, _MaxTorqueForce);

        tRig.AddTorque(tRandomTorque, ForceMode.Impulse);

        GameSceneScoreManager._Inst.AddScoreByShoot(tShootObjectGO.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID);

        _NextObjects.Enqueue(GetRandomObjectInfo());
        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);

        // 사운드
        ObjectShootSound();
    }

    private ShootChanceInfo GetRandomObjectInfo()
    {
        float tRand = Random.Range(0f, 100f);
        float tSum = 0f;

        foreach (var tInfo in _ObjectShootChance._ShootChances)
        {
            tSum += tInfo.Chance;
            if (tRand <= tSum)
            {
                return tInfo;
            }
        }

        return _ObjectShootChance._ShootChances[^1];
    }


    private void ObjectShootSound()
    {
        SoundManager._Inst.PlayRandomSFX(SoundCategory.ThrowObject);
    }
}
