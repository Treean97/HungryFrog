using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class ObjectSlicer : MonoBehaviour
{
    public Material _SlicedObjectMaterial;    // 잘린 전체 바디용 머테리얼 (디졸브 포함)
    public Material _SlicedMaterial;          // 절단면에 들어갈 전용 머테리얼
    public LayerMask _SliceMask;              // 자를 수 있는 레이어마스크

    GameObject _SlicerObject;

    float _SlicerDestroyTime = 5f;

    private void Start()
    {
        _SlicerObject = this.gameObject;
        StartCoroutine(DestroySlicerCo());
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
                
            }

            // 디졸브용 전체 바디 머테리얼 인스턴스 생성
            var tUpperMat = new Material(_SlicedObjectMaterial);
            var tLowerMat = new Material(_SlicedObjectMaterial);

            // 절단면 머테리얼(_SlicedMaterial)을 넘김
            var tUpperHullGO = tSlicedObject.CreateUpperHull(tCollider.gameObject, _SlicedMaterial);
            var tLowerHullGO = tSlicedObject.CreateLowerHull(tCollider.gameObject, _SlicedMaterial);

            tUpperHullGO.transform.position = tCollider.transform.position;
            tLowerHullGO.transform.position = tCollider.transform.position;

            var tVelocity = tCollider.GetComponent<Rigidbody>().linearVelocity;

            // 전체 머테리얼 배열 재설정 (body + cross section)
            tUpperHullGO.GetComponent<Renderer>().materials = new Material[] { tUpperMat, _SlicedMaterial };
            tLowerHullGO.GetComponent<Renderer>().materials = new Material[] { tLowerMat, _SlicedMaterial };

            // 디졸브 처리할 스크립트 추가
            tUpperHullGO.AddComponent<DissolveEffect>();
            tUpperHullGO.AddComponent<SlicedObject>();

            tLowerHullGO.AddComponent<DissolveEffect>();
            tLowerHullGO.AddComponent<SlicedObject>();

            // 물리력 부여
            ApplyPhysical(tUpperHullGO, tVelocity);
            ApplyPhysical(tLowerHullGO, tVelocity);

            // 기존 오브젝트 제거
            Destroy(tCollider.gameObject);

            // 사운드 재생
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "MainSceneObjectSlice");
        }
    }

    private void ApplyPhysical(GameObject tGO, Vector3 tVelocity)
    {
        tGO.AddComponent<BoxCollider>();
        var tRig = tGO.AddComponent<Rigidbody>();
        tRig.useGravity = true;
        tRig.linearVelocity = -tVelocity;

        float tRandomX = Random.Range(0, 3f);
        float tRandomY = Random.Range(0, 3f);
        float tRandomZ = Random.Range(0, 3f);

        tRig.AddForce(1.5f * new Vector3(tRandomX, tRandomY, tRandomZ), ForceMode.Impulse);
    }

    private SlicedHull SliceObject(GameObject tGO)
    {
        // ✅ 여기서는 절단면을 위한 cross section mat만 쓰임
        return tGO.Slice(transform.position, transform.up, _SlicedMaterial);
    }

    IEnumerator DestroySlicerCo()
    {
        yield return new WaitForSeconds(_SlicerDestroyTime);
        ObjectPoolManager._Inst.ReturnObject(this.gameObject);
    }
}
