using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class SlicerThrower : MonoBehaviour
{
    Vector3 _TargetPos;

    Vector3 _ThrowVec;
    public float _ThrowSpeed;

    [SerializeField]
    float _ClampRandomThrowAngleRange = 20f;

    [SerializeField]
    MainSceneUIManager _MainSceneUIManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInputPressed())
        {
            if(_MainSceneUIManager.GetUIIsRunning)
            {
                return;
            }

            SetTarget();
            ThrowSlicer();
        }
    }

    bool IsInputPressed()
    {
        return Input.GetMouseButtonDown(0) ||
           (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    void SetTarget()
    {
        _TargetPos = Input.mousePosition;

        // 던질 벡터 구하기
        Ray tRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        _ThrowVec = tRay.direction.normalized;

    }

    void ThrowSlicer()
    {
        // 오브젝트 가져오기
        GameObject tSlicer = ObjectPoolManager._Inst.GetObject("Slicer");

        // 오브젝트 위치 설정
        tSlicer.transform.position = Camera.main.transform.position;

        float random_x = Random.Range(-_ClampRandomThrowAngleRange, _ClampRandomThrowAngleRange);
        float random_y = Random.Range(-_ClampRandomThrowAngleRange, _ClampRandomThrowAngleRange);
        float random_z = Random.Range(-_ClampRandomThrowAngleRange, _ClampRandomThrowAngleRange);
        tSlicer.transform.rotation = Quaternion.Euler(new Vector3(random_x, random_y, random_z));
 
        // 던지기
        tSlicer.GetComponent<ThrowObject>().SetThrowVector(_ThrowVec);

        // 사운드 출력
        SoundManager._Inst.PlayRandomSFX(SoundCategory.ThrowObject);
    }

}
