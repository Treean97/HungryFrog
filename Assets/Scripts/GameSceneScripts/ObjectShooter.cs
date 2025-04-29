//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;

//public class ObjectShooter : MonoBehaviour
//{
//    public Transform _ShooterTF;        // �߻� ��ġ (ī�޶� ��ġ �Ǵ� �ٸ� ����)
//    public float _ShootForce = 0;               // �߻� ��
//    public float _PowerScale;              // �߻� �� ����
//    public float _MaxShootForce = 15f;    // �ִ� �߻� ��
//    public float _MinShootForce = 5f;    // �ִ� �߻� ��
//    public float _UpwardForce = 5f;       // ���� ��
//    public float _MaxTorqueForce;          // �ִ� ȸ�� ��
//    public float _MinTorqueForce;          // �ּ� ȸ�� ��

//    [SerializeField]
//    AdditionalForceManager _AdditionalForceManager; // �ܺ� ��

//    [SerializeField]
//    float _MaxShootDelay;

//    float _CurShootDelay;

//    bool _CanShoot = false;

//    // ��ġ üũ
//    bool _IsPressed = false;
//    bool _IsReleased = false;

//    // �߻� �� ������Ʈ�� (���� �ܰ踸 �߻�)
//    [SerializeField]
//    GameObject[] _ShootObjects;

//    [SerializeField]
//    ObjectShootChance _ObjectShootChance;

//    Queue<ShootChanceInfo> _NextObjects = new Queue<ShootChanceInfo>();

//    [SerializeField]
//    GameSceneNextObjectsUI _NextObjectsUI;

//    private void Start()
//    {
//        _ShootForce = 0;

//        // �ܺ� �� ����
//        _AdditionalForceManager.SetAdditionalForce();

//        // �߻� ������ ����
//        _CurShootDelay = _MaxShootDelay;

//        for (int i = 0; i < _NextObjectsUI._SlotImages.Count; i++)
//        {
//            _NextObjects.Enqueue(GetRandomObjectInfo());
//        }

//        // UI ����
//        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);
//    }

//    void Update()
//    {

//        if (_CurShootDelay < _MaxShootDelay)
//        {
//            _CurShootDelay += Time.deltaTime;
//        }
//        else
//        {
//            _CanShoot = true;
//        }

//        _IsPressed = Input.GetKey(KeyCode.Space) || IsTouchPressed();
//        _IsReleased = Input.GetKeyUp(KeyCode.Space) || IsTouchReleased();

//        if (_IsPressed && _CanShoot)
//        {
//            // _LineRenderer.enabled = false;

//            SettingShootPower();
//        }

//        if (_IsReleased && _CanShoot)
//        {
//            // �߻�
//            ShootObject();

//            // �ܺ� �� ����
//            _AdditionalForceManager.SetAdditionalForce();
//        }
//    }
//    bool IsTouchPressed()
//    {
//        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary;
//    }

//    bool IsTouchReleased()
//    {
//        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
//    }


//    void SettingShootPower()
//    {
//        _ShootForce += Time.deltaTime * _PowerScale;

//    }

//    void ShootObject()
//    {
//        // 1. ť���� ����
//        ShootChanceInfo tSelectedInfo = _NextObjects.Dequeue();

//        // ������Ʈ Ǯ���� �ش� �̸����� ��������
//        GameObject tShootObjectGO = ObjectPoolManager._Inst.GetObject(tSelectedInfo.ShootObjectsData.GetShootObjectName.ToString());

//        // ��ġ �� �߻� ó��
//        tShootObjectGO.transform.position = _ShooterTF.position;
//        Rigidbody tRig = tShootObjectGO.GetComponent<Rigidbody>();



//        Vector3 tForce = _ShooterTF.forward * (_ShootForce + _MinShootForce)
//                       + Vector3.up * _UpwardForce
//                       + _AdditionalForceManager.GetAdditionalForceVector;

//        // �ִ� �ּ� ����
//        float clampedMagnitude = Mathf.Clamp(tForce.magnitude, _MinShootForce, _MaxShootForce);
//        tForce = tForce.normalized * clampedMagnitude;

//        tRig.AddForce(tForce, ForceMode.Impulse);

//        _ShootForce = 0;
//        _CanShoot = false;
//        _CurShootDelay = 0;

//        // ���� ȸ���� (XYZ ���� -1 ~ 1 ���� ���⿡ ���Ƿ� �� �ֱ�)
//        Vector3 tRandomTorque = new Vector3(
//            Random.Range(-1f, 1f),
//            Random.Range(-1f, 1f),
//            Random.Range(-1f, 1f)
//        ).normalized * Random.Range(_MinTorqueForce, _MaxTorqueForce); // ȸ�� ���� ����

//        tRig.AddTorque(tRandomTorque, ForceMode.Impulse);

//        GameSceneScoreManager._Inst.AddScoreByShoot(tShootObjectGO.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID);

//        // ���� ������Ʈ ä���
//         _NextObjects.Enqueue(GetRandomObjectInfo());

//        // UI ����
//        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);
//    }

//    ShootChanceInfo GetRandomObjectInfo()
//    {
//        float tRand = Random.Range(0f, 100f);

//        float tSum = 0f;


//        // 0~apple chance : apple
//        // apple chance ~ apple chance + avocado chance : avocado
//        // ...

//        foreach (var tInfo in _ObjectShootChance._ShootChances)
//        {
//            tSum += tInfo.Chance;
//            if (tRand <= tSum)
//            {
//                return tInfo;
//            }
//        }

//        // Ȥ�ö� ��Ȯ�� 100�� ���� ������ ��� ���� ó��
//        return _ObjectShootChance._ShootChances[^1];
//    }

//}

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class ObjectShooter : MonoBehaviour
//{
//    [Header("Shooter ����")]
//    public Transform _ShooterTF;
//    public float _ShootForce = 0;
//    public float _PowerScale;
//    public float _MaxShootForce = 15f;
//    public float _MinShootForce = 5f;
//    public float _UpwardForce = 5f;
//    public float _MaxTorqueForce;
//    public float _MinTorqueForce;

//    [SerializeField] private AdditionalForceManager _AdditionalForceManager;
//    [SerializeField] private float _MaxShootDelay;
//    [SerializeField] private GameObject[] _ShootObjects;
//    [SerializeField] private ObjectShootChance _ObjectShootChance;
//    [SerializeField] private GameSceneNextObjectsUI _NextObjectsUI;

//    [Header("���̽�ƽ ����")]
//    [SerializeField] private RectTransform _JoystickBackground; // ���̽�ƽ ��� �߰�

//    private float _CurShootDelay;
//    private bool _CanShoot = false;
//    private bool _IsPressed = false;
//    private bool _IsReleased = false;
//    private Queue<ShootChanceInfo> _NextObjects = new Queue<ShootChanceInfo>();

//    private void Start()
//    {
//        _ShootForce = 0;
//        _AdditionalForceManager.SetAdditionalForce();
//        _CurShootDelay = _MaxShootDelay;

//        for (int i = 0; i < _NextObjectsUI._SlotImages.Count; i++)
//        {
//            _NextObjects.Enqueue(GetRandomObjectInfo());
//        }

//        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);
//    }

//    private void Update()
//    {
//        if (_CurShootDelay < _MaxShootDelay)
//        {
//            _CurShootDelay += Time.deltaTime;
//        }
//        else
//        {
//            _CanShoot = true;
//        }

//        _IsPressed = Input.GetKey(KeyCode.Space) || IsTouchPressed();
//        _IsReleased = Input.GetKeyUp(KeyCode.Space) || IsTouchReleased();

//        if (_IsPressed && _CanShoot)
//        {
//            SettingShootPower();
//        }

//        if (_IsReleased && _CanShoot)
//        {
//            ShootObject();
//            _AdditionalForceManager.SetAdditionalForce();
//        }
//    }

//    private bool IsTouchPressed()
//    {
//        if (Input.touchCount > 0)
//        {
//            var touch = Input.GetTouch(0);

//            if (IsTouchOnJoystick(touch.position))
//                return false; // ���̽�ƽ ��ġ�� ����

//            return touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary;
//        }

//        return false;
//    }

//    private bool IsTouchReleased()
//    {
//        if (Input.touchCount > 0)
//        {
//            var touch = Input.GetTouch(0);

//            if (IsTouchOnJoystick(touch.position))
//                return false; // ���̽�ƽ ��ġ�� ����

//            return touch.phase == TouchPhase.Ended;
//        }

//        return false;
//    }

//    private bool IsTouchOnJoystick(Vector2 touchPosition)
//    {
//        if (_JoystickBackground == null)
//            return false;

//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            _JoystickBackground,
//            touchPosition,
//            null,
//            out Vector2 localPoint
//        );

//        Vector2 halfSize = _JoystickBackground.sizeDelta * 0.5f;

//        return Mathf.Abs(localPoint.x) <= halfSize.x && Mathf.Abs(localPoint.y) <= halfSize.y;
//    }

//    private void SettingShootPower()
//    {
//        _ShootForce += Time.deltaTime * _PowerScale;
//    }

//    private void ShootObject()
//    {
//        ShootChanceInfo tSelectedInfo = _NextObjects.Dequeue();
//        GameObject tShootObjectGO = ObjectPoolManager._Inst.GetObject(tSelectedInfo.ShootObjectsData.GetShootObjectName.ToString());

//        tShootObjectGO.transform.position = _ShooterTF.position;
//        Rigidbody tRig = tShootObjectGO.GetComponent<Rigidbody>();

//        Vector3 tForce = _ShooterTF.forward * (_ShootForce + _MinShootForce)
//                       + Vector3.up * _UpwardForce
//                       + _AdditionalForceManager.GetAdditionalForceVector;

//        float clampedMagnitude = Mathf.Clamp(tForce.magnitude, _MinShootForce, _MaxShootForce);
//        tForce = tForce.normalized * clampedMagnitude;

//        tRig.AddForce(tForce, ForceMode.Impulse);

//        _ShootForce = 0;
//        _CanShoot = false;
//        _CurShootDelay = 0;

//        Vector3 tRandomTorque = new Vector3(
//            Random.Range(-1f, 1f),
//            Random.Range(-1f, 1f),
//            Random.Range(-1f, 1f)
//        ).normalized * Random.Range(_MinTorqueForce, _MaxTorqueForce);

//        tRig.AddTorque(tRandomTorque, ForceMode.Impulse);

//        GameSceneScoreManager._Inst.AddScoreByShoot(
//            tShootObjectGO.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID
//        );

//        _NextObjects.Enqueue(GetRandomObjectInfo());
//        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);
//    }

//    private ShootChanceInfo GetRandomObjectInfo()
//    {
//        float tRand = Random.Range(0f, 100f);
//        float tSum = 0f;

//        foreach (var tInfo in _ObjectShootChance._ShootChances)
//        {
//            tSum += tInfo.Chance;
//            if (tRand <= tSum)
//            {
//                return tInfo;
//            }
//        }

//        return _ObjectShootChance._ShootChances[^1];
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class ObjectShooter : MonoBehaviour
{
    [Header("Shooter ����")]
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
    [SerializeField] private GameObject[] _ShootObjects;
    [SerializeField] private ObjectShootChance _ObjectShootChance;
    [SerializeField] private GameSceneNextObjectsUI _NextObjectsUI;

    private float _CurShootDelay;
    private bool _CanShoot = false;
    private bool _IsPressed = false;
    private bool _IsReleased = false;
    private Queue<ShootChanceInfo> _NextObjects = new Queue<ShootChanceInfo>();

    private int _ShootTouchID = -1; // �߻�� ��ġ ID

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

    private bool IsTouchPressed()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);

            if (touch.fingerId == GameSceneController.JoystickTouchID)
                continue; // ���̽�ƽ ��ġ�� ����

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

        GameSceneScoreManager._Inst.AddScoreByShoot(
            tShootObjectGO.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID
        );

        _NextObjects.Enqueue(GetRandomObjectInfo());
        _NextObjectsUI.UpdateQueueDisplay(_NextObjects);
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
}
