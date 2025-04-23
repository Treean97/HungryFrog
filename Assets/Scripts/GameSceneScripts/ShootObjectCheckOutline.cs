using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootObjectCheckOutline : MonoBehaviour
{
    GameObject _OutlineShpere; // �ܺζ���
    ShootObjectBasement _ShootObjectBasement;
    float _OutlineRadius;
    float _ObjectRadius;

    void Start()
    {
        /* �׽�Ʈ �ڵ�
         * 
         * 
        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // �ܺζ����� ������ ���
        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

        // ������Ʈ�� ������ ��� (�ݶ��̴� ����)
        SphereCollider tSphereCollider = GetComponent<SphereCollider>();
        _ObjectRadius = tSphereCollider.radius * transform.localScale.x;
        */

        _OutlineShpere = GameObject.FindGameObjectWithTag("OutlineSphere");
        _ShootObjectBasement = GetComponent<ShootObjectBasement>();

        // �ܺζ����� ������ ���
        _OutlineRadius = _OutlineShpere.transform.localScale.x * 0.5f;

        // ������Ʈ�� ������ ��� (MeshCollider ����, �뷫��)
        MeshCollider tMeshCollider = GetComponent<MeshCollider>();
        _ObjectRadius = tMeshCollider.bounds.extents.magnitude;

    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(this.transform.position, Vector3.zero));

        // ������Ʈ�� ���� ũ�⸦ ������� Ȯ�� && ���°� Stable���� Ȯ�� , * 10�� �����ϸ�
        if (Vector2.Distance(this.transform.position, Vector3.zero) - _ObjectRadius > _OutlineRadius
            && _ShootObjectBasement.GetIsStable == true)
        {
            Debug.Log("������Ʈ�� ���� ������ϴ�! ���� ����!");
            Debug.Log(this.gameObject);
            Time.timeScale = 0f;
        }
    }
}
