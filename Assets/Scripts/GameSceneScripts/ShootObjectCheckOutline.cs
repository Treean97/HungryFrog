using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObjectCheckOutline : MonoBehaviour
{
    // 1) 참조할 오브젝트
    GameObject _OutlineSphere;            // 테두리 역할 구체
    GameObject _ZeroPointSphere;          // 중심 역할 구체
    ShootObjectBasement _ShootObjectBasement; // 투사체 베이스 컴포넌트

    // 2) 반지름 저장용
    float _OutlineRadius; // 테두리 구체 반경
    float _ObjectRadius;  // 투사체 반경

    GameSceneManager _GameSceneManager; // 씬 매니저 참조

    void Start()
    {
        // 참조 오브젝트 찾기
        _ZeroPointSphere = GameObject.FindGameObjectWithTag("ZeroPointSphere");
        _OutlineSphere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // 3) OutlineSphere 반경 계산 (Collider 사용)
        SphereCollider outlineCol = _OutlineSphere.GetComponent<SphereCollider>();
        if (outlineCol != null)
        {
            // 콜라이더 반경에 스케일 곱해서 최종 반경 구함
            _OutlineRadius = outlineCol.radius * _OutlineSphere.transform.localScale.x;
        }
        else
        {
            // 콜라이더 없으면 스케일 기준으로 대략 반경 계산
            _OutlineRadius = _OutlineSphere.transform.localScale.x * 0.5f;
        }

        // 4) 투사체 반경 계산 (SphereCollider 또는 MeshCollider)
        SphereCollider meshCol = GetComponent<SphereCollider>();
        if (meshCol != null)
        {
            _ObjectRadius = meshCol.radius * transform.localScale.x;
        }
        else
        {
            MeshCollider fallback = GetComponent<MeshCollider>();
            _ObjectRadius = fallback.bounds.extents.magnitude;
        }

        // 씬 매니저 가져오기
        _GameSceneManager = GameObject.FindGameObjectWithTag("GameSceneManager")
                                .GetComponent<GameSceneManager>();
    }

    void Update()
    {
        // 5) 중심 구체와 투사체 간 거리 계산
        float currentDistance = Vector3.Distance(
            transform.position,
            _ZeroPointSphere.transform.position
        );

        // 6) 거리 > (테두리 반경 + 투사체 반경) 이고 안정 상태면 엔딩 호출
        if (currentDistance > _OutlineRadius + _ObjectRadius
            && _ShootObjectBasement.GetIsStable)
        {
            _GameSceneManager.Ending(); // 게임 씬 엔딩 처리
        }
    }
}
