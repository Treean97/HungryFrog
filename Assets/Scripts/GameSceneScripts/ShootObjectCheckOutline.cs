using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootObjectCheckOutline : MonoBehaviour
{
    GameObject _OutlineShpere; // 외부라인
    ShootObjectBasement _ShootObjectBasement;
    float _OutlineRadius;
    float _ObjectRadius;

    void Start()
    {
        /* 테스트 코드
         * 
         * 
        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // 외부라인의 반지름 계산
        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

        // 오브젝트의 반지름 계산 (콜라이더 기준)
        SphereCollider tSphereCollider = GetComponent<SphereCollider>();
        _ObjectRadius = tSphereCollider.radius * transform.localScale.x;
        */

        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // 외부라인의 반지름 계산
        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

        // 오브젝트의 반지름 계산 (MeshCollider 기준, 대략적)
        MeshCollider tMeshCollider = GetComponent<MeshCollider>();
        _ObjectRadius = tMeshCollider.bounds.extents.magnitude;

    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(this.transform.position, Vector3.zero));

        // 오브젝트가 원의 크기를 벗어나는지 확인 && 상태가 Stable인지 확인 , * 10은 스케일링
        if (Vector2.Distance(this.transform.position, Vector3.zero) - _ObjectRadius > _OutlineRadius
            && _ShootObjectBasement.GetIsStable == true)
        {
            Debug.Log("오브젝트가 원을 벗어났습니다! 게임 종료!");
            Debug.Log(this.gameObject);
            Time.timeScale = 0f;
        }
    }
}
