using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCamera : MonoBehaviour
{
    public Transform _Target; // ī�޶� ���� Ÿ��
    public float _RotationSpeed; // ȸ�� �ӵ�
    public float _DistanceFromTarget; // Ÿ�����κ����� �Ÿ�
    public float _HeightOffset; // Ÿ�ٰ� ī�޶� ���� ���� ����

    private float _CurrentAngle; // ���� ȸ�� ����

    void Start()
    {
        // ī�޶��� �ʱ� ���� ����
        _CurrentAngle = transform.eulerAngles.y; // Y�� �ʱ�ȭ
    }

    void Update()
    {
        if (_Target)
        {
            // ���콺 �Է¿� ���� Y�� ȸ�� ���� ������Ʈ
            _CurrentAngle += Input.GetAxis("Mouse X") * _RotationSpeed;

            // ī�޶��� ���ο� ��ġ ���
            Vector3 offset = new Vector3(Mathf.Sin(_CurrentAngle * Mathf.Deg2Rad), 0, Mathf.Cos(_CurrentAngle * Mathf.Deg2Rad)) * _DistanceFromTarget;
            transform.position = _Target.position + offset + Vector3.up * _HeightOffset;

            // ī�޶� Ÿ���� �ٶ󺸵��� ����
            transform.LookAt(_Target.position);
        }
    }
}
