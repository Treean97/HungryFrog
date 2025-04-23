using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlicerThrower : MonoBehaviour
{
    Vector3 _TargetPos;

    Vector3 _ThrowVec;
    public float _ThrowSpeed;
    float _ThrowAngleRange = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTarget();
            ThrowSlicer();
        }
    }

    void SetTarget()
    {
        _TargetPos = Input.mousePosition;

        // ���� ���� ���ϱ�
        Ray tRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        _ThrowVec = tRay.direction.normalized;

    }

    void ThrowSlicer()
    {
        // ������Ʈ ��������
        GameObject tSlicer = ObjectPoolManager._Inst.GetObject("Slicer");

        // ������Ʈ ��ġ ����
        tSlicer.transform.position = Camera.main.transform.position;

        float random_x = Random.Range(-45, 45);
        float random_y = Random.Range(-45, 45);
        float random_z = Random.Range(-45, 45);
        tSlicer.transform.rotation = Quaternion.Euler(new Vector3(random_x, random_y, random_z));
 
        // ������
        tSlicer.GetComponent<ThrowObject>().SetThrowVector(_ThrowVec);
    }

}
