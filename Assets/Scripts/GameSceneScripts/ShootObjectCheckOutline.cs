//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;

//public class ShootObjectCheckOutline : MonoBehaviour
//{
//    GameObject _OutlineShpere; // �ܺζ���
//    GameObject _ZeroPointSphere; // �߽ɱ�
//    ShootObjectBasement _ShootObjectBasement;
//    float _OutlineRadius;
//    float _ObjectRadius;

//    GameSceneManager _GameSceneManager;

//    void Start()
//    {
//        _ZeroPointSphere = GameObject.FindGameObjectWithTag("ZeroPointSphere");
//        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
//        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

//        // �ܺζ����� ������ ���
//        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

//        // ������Ʈ�� ������ ��� (MeshCollider ����, �뷫��)
//        MeshCollider tMeshCollider = GetComponent<MeshCollider>();
//        _ObjectRadius = tMeshCollider.bounds.extents.magnitude;

//        _GameSceneManager = GameObject.FindGameObjectWithTag("GameSceneManager").GetComponent<GameSceneManager>();
//    }

//    private void Update()
//    {
//        //Debug.Log(Vector2.Distance(this.transform.position, Vector3.zero));

//        // ������Ʈ�� ���� ũ�⸦ ������� Ȯ�� && ���°� Stable���� Ȯ��
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
    GameObject _OutlineSphere;      // �ܰ� ��(�ƿ�����)
    GameObject _ZeroPointSphere;    // �߽� ��
    ShootObjectBasement _ShootObjectBasement;

    float _OutlineRadius;
    float _ObjectRadius;

    GameSceneManager _GameSceneManager;

    void Start()
    {
        _ZeroPointSphere = GameObject.FindGameObjectWithTag("ZeroPointSphere");
        _OutlineSphere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // OutlineSphere ������ ���
        SphereCollider outlineCol = _OutlineSphere.GetComponent<SphereCollider>();
        if (outlineCol != null)
        {
            _OutlineRadius = outlineCol.radius * _OutlineSphere.transform.localScale.x;
        }
        else
        {
            _OutlineRadius = _OutlineSphere.transform.localScale.x * 0.5f;
        }

        // �ڽ��� ������ ���
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
        // 3) 3D �Ÿ� ��� (Vector3.Distance ���)
        float currentDistance = Vector3.Distance(
            transform.position,
            _ZeroPointSphere.transform.position
        );

        // 4) �������� ����� ���� ����: 
        //    �߽� �� �Ÿ� > (�ƿ����� ������ + ������Ʈ ������)
        if (currentDistance > _OutlineRadius + _ObjectRadius
            && _ShootObjectBasement.GetIsStable)
        {
            _GameSceneManager.Ending();
        }
    }
}

