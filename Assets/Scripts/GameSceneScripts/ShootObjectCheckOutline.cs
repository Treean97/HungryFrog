using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootObjectCheckOutline : MonoBehaviour
{
    GameObject _OutlineShpere; // �ܺζ���
    GameObject _ZeroPointSphere; // �߽ɱ�
    ShootObjectBasement _ShootObjectBasement;
    float _OutlineRadius;
    float _ObjectRadius;

    GameSceneManager _GameSceneManager;

    void Start()
    {
        _ZeroPointSphere = GameObject.FindGameObjectWithTag("ZeroPointSphere");
        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // �ܺζ����� ������ ���
        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

        // ������Ʈ�� ������ ��� (MeshCollider ����, �뷫��)
        MeshCollider tMeshCollider = GetComponent<MeshCollider>();
        _ObjectRadius = tMeshCollider.bounds.extents.magnitude;

        _GameSceneManager = GameObject.FindGameObjectWithTag("GameSceneManager").GetComponent<GameSceneManager>();
    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(this.transform.position, Vector3.zero));

        // ������Ʈ�� ���� ũ�⸦ ������� Ȯ�� && ���°� Stable���� Ȯ�� , * 10�� �����ϸ�
        if (Vector2.Distance(this.transform.position, _ZeroPointSphere.transform.position) - _ObjectRadius > _OutlineRadius
            && _ShootObjectBasement.GetIsStable == true)
        {
            _GameSceneManager.Ending();
        }
    }
}
