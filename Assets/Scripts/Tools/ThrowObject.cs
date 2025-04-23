using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class ThrowObject : MonoBehaviour
{
    public float _ThrowForce = 10f; // 던질 힘
    public float _Gravity = -10f; // 중력 값

    Vector3 _ThrowVector; // 던질 방향
    Vector3 _Velocity; // 현재 속도

    public float _RotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 회전
        RotateAroundLocalX();

        // 이동
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
        // 속도 설정
        _Velocity = _ThrowVector * _ThrowForce;
    }

    private void ApplyGravity()
    {
        // 중력 적용
        _Velocity.y += _Gravity * Time.deltaTime; // 중력 효과 추가
        transform.position += _Velocity * Time.deltaTime; // 위치 업데이트
    }

}

