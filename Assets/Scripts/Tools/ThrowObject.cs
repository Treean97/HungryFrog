using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class ThrowObject : MonoBehaviour
{
    public float _ThrowForce = 10f; // ���� ��
    public float _Gravity = -10f; // �߷� ��

    Vector3 _ThrowVector; // ���� ����
    Vector3 _Velocity; // ���� �ӵ�

    public float _RotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ȸ��
        RotateAroundLocalX();

        // �̵�
        Throw();
        ApplyGravity();
    }

    void RotateAroundLocalX()
    {
        float tRotationAmount = _RotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * tRotationAmount);
    }

    public void SetThrowVector(Vector3 tVector)
    {
        _ThrowVector = tVector;
    }

    public void Throw()
    {
        // �ӵ� ����
        _Velocity = _ThrowVector * _ThrowForce;
    }

    private void ApplyGravity()
    {
        // �߷� ����
        _Velocity.y += _Gravity * Time.deltaTime; // �߷� ȿ�� �߰�
        transform.position += _Velocity * Time.deltaTime; // ��ġ ������Ʈ
    }

}

