using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalForceManager : MonoBehaviour
{
    [SerializeField] AdditionalForceUI _AdditionalForceUI;   // 추가 힘 UI 참조
    [SerializeField] float _MaxRandomForceRange;             // 최대 랜덤 힘 범위

    float _AdditionalForce = 0;      // 계산된 힘 크기
    float _MinAdditionalForce = 0;   // 최소 힘 크기

    Vector3 _AdditionalForceVector = Vector3.zero;  
    public Vector3 GetAdditionalForceVector => _AdditionalForceVector;  // 최종 힘 벡터

    public float GetFixedAdditionalForce => _AdditionalForce;           // 계산된 힘 크기 반환

    // 힘 방향 배열 (인덱스별 방향)
    Vector3[] _ForceDirection = new Vector3[]
    {
        new Vector3(0,0,0),
        new Vector3(0,1,0),
        new Vector3(1,1,0),
        new Vector3(1,0,0),
        new Vector3(1,-1,0),
        new Vector3(0,-1,0),
        new Vector3(-1,-1,0),
        new Vector3(-1,0,0),
        new Vector3(-1,1,0)
    };

    void Start() { }

    void Update() { }

    // 1) 추가 힘 설정 호출
    public void SetAdditionalForce()
    {
        int tRandomInt = Random.Range(0, _ForceDirection.Length);           // 방향 인덱스 결정
        float tRandomFloat = 0;

        if (tRandomInt != 0)
        {
            tRandomFloat = Random.Range(_MinAdditionalForce, _MaxRandomForceRange);  
            _AdditionalForce = Mathf.Floor(tRandomFloat * 10f) / 10f;        // 소수점 첫째자리 절사
        }
        else
        {
            _AdditionalForce = 0;                                           // 인덱스 0일 땐 힘 0
        }

        _AdditionalForceVector = _AdditionalForce * _ForceDirection[tRandomInt];  
        _AdditionalForceUI.SetAdditionalForceText(_AdditionalForce);        // UI 텍스트 갱신
        _AdditionalForceUI.SetAdditionalForceImage(tRandomInt);             // UI 이미지 갱신
    }
}
