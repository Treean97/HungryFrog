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

    // ��ġ �� Ŭ�� ����
    bool IsInputPressed()
    {
        // ����� ��ġ
        if (Input.touchCount > 0)
        {
            Touch tTouch = Input.GetTouch(0);

            if (tTouch.phase == TouchPhase.Began)
            {
                // UI �� ��ġ�� ����
                if (EventSystem.current.IsPointerOverGameObject(tTouch.fingerId))
                    return false;

                return true;
            }
        }
        // PC ���콺 Ŭ��
        else if (Input.GetMouseButtonDown(0))
        {
            // UI �� Ŭ���̸� ����
            if (EventSystem.current.IsPointerOverGameObject())
                return false;

            return true;
        }

        return false;
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

        float random_x = Random.Range(-_ClampRandomThrowAngleRange, _ClampRandomThrowAngleRange);
        float random_y = Random.Range(-_ClampRandomThrowAngleRange, _ClampRandomThrowAngleRange);
        float random_z = Random.Range(-_ClampRandomThrowAngleRange, _ClampRandomThrowAngleRange);
        tSlicer.transform.rotation = Quaternion.Euler(new Vector3(random_x, random_y, random_z));
 
        // ������
        tSlicer.GetComponent<ThrowObject>().SetThrowVector(_ThrowVec);

        // ���� ���
        SoundManager._Inst.PlayRandomSFX(SoundCategory.ThrowObject);
    }

}
