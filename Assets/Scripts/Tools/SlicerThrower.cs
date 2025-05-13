using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


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

    // 터치 및 클릭 감지
    bool IsInputPressed()
    {
        // 모바일 터치
        if (Input.touchCount > 0)
        {
            Touch tTouch = Input.GetTouch(0);

            if (tTouch.phase == TouchPhase.Began)
            {
                // UI 위 터치면 무시
                if (EventSystem.current.IsPointerOverGameObject(tTouch.fingerId))
                    return false;

                return true;
            }
        }
        // PC 마우스 클릭
        else if (Input.GetMouseButtonDown(0))
        {
            // UI 위 클릭이면 무시
            if (EventSystem.current.IsPointerOverGameObject())
                return false;

            return true;
        }

        return false;
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
