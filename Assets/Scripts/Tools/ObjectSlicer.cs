using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Collections.Specialized;

public class ObjectSlicer : MonoBehaviour
{
    public Material _SlicedMaterial;    // 잘린 부분의 머테리얼
    public LayerMask _SliceMask;        // 자를 수 있는 레이어마스크

    GameObject _SlicerObject;

    float _SlicerDestroyTime = 5f;
    float _SlicedObjectDestroyTime = 5f;

    private void Start()
    {
        _SlicerObject = this.gameObject;
        StartCoroutine(DestroySlicerCoroutine());
    }

    private void Update()
    {
        Slice();
    }
    
    public void Slice()
    {
        Collider[] tObjectsToSlice = Physics.OverlapBox(transform.position, _SlicerObject.transform.localScale,
        transform.rotation, _SliceMask);

        foreach (Collider tCollider in tObjectsToSlice)
        {
            SlicedHull tSlicedObject = SliceObject(tCollider.gameObject);

            if (tSlicedObject == null)
            {
                Debug.LogError("SlicedHull 객체가 null입니다. SliceObject 메서드가 제대로 작동하지 않았습니다.");
                return;
            }

            var tUpperHullGO = tSlicedObject.CreateUpperHull(tCollider.gameObject, _SlicedMaterial);
            var tLowerHullGO = tSlicedObject.CreateLowerHull(tCollider.gameObject, _SlicedMaterial);

            tUpperHullGO.transform.position = tCollider.transform.position;
            tLowerHullGO.transform.position = tCollider.transform.position;

            var tVelocity = tCollider.GetComponent<Rigidbody>().linearVelocity;        

            ApplyPhysical(tUpperHullGO, tVelocity);
            ApplyPhysical(tLowerHullGO, tVelocity);

            // 기존 오브젝트 파괴
            Destroy(tCollider.gameObject);

            // 일정 시간 이후 조각난 오브젝트 파괴
            StartCoroutine(DestroySlicedObjectCoroutine(tUpperHullGO));
            StartCoroutine(DestroySlicedObjectCoroutine(tLowerHullGO));            
        }
    }

    private void ApplyPhysical(GameObject tGO, Vector3 tVelocity)
    {
        //tGO.AddComponent<MeshCollider>().convex = true;
        tGO.AddComponent<BoxCollider>();
        var tRig = tGO.AddComponent<Rigidbody>();
        // test
        tRig.useGravity = true;
        tRig.linearVelocity = -tVelocity;

        float tRandomX = Random.Range(0, 3f);
        float tRandomY = Random.Range(0, 3f);
        float tRandomZ = Random.Range(0, 3f);

        tRig.AddForce(1.5f * new Vector3(tRandomX, tRandomY, tRandomZ), ForceMode.Impulse);

    }

    
    private SlicedHull SliceObject(GameObject tGO)
    {
        return tGO.Slice(transform.position, transform.up, _SlicedMaterial);
    }
    
    IEnumerator DestroySlicerCoroutine()
    {
        yield return new WaitForSeconds(_SlicerDestroyTime);
        ObjectPoolManager._Inst.ReturnObject(this.gameObject);
    }

    IEnumerator DestroySlicedObjectCoroutine(GameObject tDestroyObject)
    {
        yield return new WaitForSeconds(_SlicedObjectDestroyTime);
        ObjectPoolManager._Inst.ReturnObject(tDestroyObject);
    }
}