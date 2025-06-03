//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;

//public class ShootObjectCheckOutline : MonoBehaviour
//{
//    GameObject _OutlineShpere; // 외부라인
//    GameObject _ZeroPointSphere; // 중심구
//    ShootObjectBasement _ShootObjectBasement;
//    float _OutlineRadius;
//    float _ObjectRadius;

//    GameSceneManager _GameSceneManager;

//    void Start()
//    {
//        _ZeroPointSphere = GameObject.FindGameObjectWithTag("ZeroPointSphere");
//        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
//        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

//        // 외부라인의 반지름 계산
//        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

//        // 오브젝트의 반지름 계산 (MeshCollider 기준, 대략적)
//        MeshCollider tMeshCollider = GetComponent<MeshCollider>();
//        _ObjectRadius = tMeshCollider.bounds.extents.magnitude;

//        _GameSceneManager = GameObject.FindGameObjectWithTag("GameSceneManager").GetComponent<GameSceneManager>();
//    }

//    private void Update()
//    {
//        //Debug.Log(Vector2.Distance(this.transform.position, Vector3.zero));

//        // 오브젝트가 원의 크기를 벗어나는지 확인 && 상태가 Stable인지 확인
//        if (Vector3.Distance(this.transform.position, _ZeroPointSphere.transform.position) - _ObjectRadius > _OutlineRadius
//            && _ShootObjectBasement.GetIsStable == true)
//        {
//            _GameSceneManager.Ending();
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObjectCheckOutline : MonoBehaviour
{
    GameObject _OutlineSphere;      // 외곽 구(아웃라인)
    GameObject _ZeroPointSphere;    // 중심 구
    ShootObjectBasement _ShootObjectBasement;

    float _OutlineRadius;
    float _ObjectRadius;

    GameSceneManager _GameSceneManager;

    void Start()
    {
        _ZeroPointSphere = GameObject.FindGameObjectWithTag("ZeroPointSphere");
        _OutlineSphere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // OutlineSphere 반지름 계산
        SphereCollider outlineCol = _OutlineSphere.GetComponent<SphereCollider>();
        if (outlineCol != null)
        {
            _OutlineRadius = outlineCol.radius * _OutlineSphere.transform.localScale.x;
        }
        else
        {
            _OutlineRadius = _OutlineSphere.transform.localScale.x * 0.5f;
        }

        // 자신의 반지름 계산
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

        _GameSceneManager = GameObject.FindGameObjectWithTag("GameSceneManager")
                                .GetComponent<GameSceneManager>();
    }

    void Update()
    {
        // 3) 3D 거리 계산 (Vector3.Distance 사용)
        float currentDistance = Vector3.Distance(
            transform.position,
            _ZeroPointSphere.transform.position
        );

        // 4) “완전히 벗어났을 때” 판정: 
        //    중심 간 거리 > (아웃라인 반지름 + 오브젝트 반지름)
        if (currentDistance > _OutlineRadius + _ObjectRadius
            && _ShootObjectBasement.GetIsStable)
        {
            _GameSceneManager.Ending();
        }
    }
}

