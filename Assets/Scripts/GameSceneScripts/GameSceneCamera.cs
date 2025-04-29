using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

//public class GameSceneCamera : MonoBehaviour
//{
//    public Transform _Target; // 카메라가 따라갈 타겟
//    public float _RotationSpeed; // 회전 속도
//    public float _DistanceFromTarget; // 타겟으로부터의 거리
//    public float _HeightOffset; // 타겟과 카메라 간의 높이 차이

//    private float _CurrentAngle; // 현재 회전 각도

//    void Start()
//    {
//        // 카메라의 초기 각도 설정
//        _CurrentAngle = transform.eulerAngles.y; // Y축 초기화
//    }

//    void Update()
//    {
//        if (_Target)
//        {
//            // 마우스 입력에 따라 Y축 회전 각도 업데이트
//            _CurrentAngle += Input.GetAxis("Mouse X") * _RotationSpeed;

//            // 카메라의 새로운 위치 계산
//            Vector3 offset = new Vector3(Mathf.Sin(_CurrentAngle * Mathf.Deg2Rad), 0, Mathf.Cos(_CurrentAngle * Mathf.Deg2Rad)) * _DistanceFromTarget;
//            transform.position = _Target.position + offset + Vector3.up * _HeightOffset;

//            // 카메라가 타겟을 바라보도록 설정
//            transform.LookAt(_Target.position);
//        }
//    }
//}