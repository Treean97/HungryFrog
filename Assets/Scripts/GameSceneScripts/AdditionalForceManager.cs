using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalForceManager : MonoBehaviour
{
    [SerializeField]
    AdditionalForceUI _AdditionalForceUI;   // UI 

    [SerializeField]
    float _MaxRandomForceRange;   // 랜덤 값 범위  

    float _AdditionalForce = 0;
    float _MinAdditionalForce = 0;


    Vector3 _AdditonalForceVector = Vector3.zero;
    public Vector3 GetAdditionalForceVector
    {
        get { return _AdditonalForceVector; }
    }


    public float GetFixedAdditionalForce
    {
        get { return _AdditionalForce; }
    }

    // 방향
    // 0 : Vector3 0,0,0
    // 1 : Vector3 0,1,0
    // 2 : Vector3 1,1,0
    // 3 : Vector3 1,0,0
    // 4 : Vector3 1,-1,0
    // 5 : Vector3 0,-1,0
    // 6 : Vector3 -1,-1,0
    // 7 : Vector3 -1,0,0
    // 8 : Vector3 -1,1,0


    Vector3[] _ForceDirection = new Vector3[]
    {
        new Vector3(0,0,0),
        new Vector3(0,1,0),
        new Vector3(1,1,0),
        new Vector3(1,0,0),
        new Vector3(1,-1,0),
        new Vector3(0,-1,0),
        new Vector3(-1,-1,0),
        new Vector3(-1,0,0),
        new Vector3(-1,1,0)
    };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAdditionalForce()
    {
        // 랜덤 방향 추출
        int tRandomInt = Random.Range(0, _ForceDirection.Length);

        // 힘 크기
        float tRandomfloat = 0;

        // 방향이 0 이면 외부힘 X
        if (tRandomInt != 0)
        {
            // 랜덤 힘 크기 추출
            tRandomfloat = Random.Range(_MinAdditionalForce, _MaxRandomForceRange);
            // 소숫점 1자리까지 남기고 버림
            _AdditionalForce = Mathf.Floor(tRandomfloat * 10f) / 10f;
        }     
        else
        {
            _AdditionalForce = tRandomfloat;
        }

        // 방향 * 크기 벡터로 변환
        _AdditonalForceVector = _AdditionalForce * _ForceDirection[tRandomInt];

        // UI Text 표시
        _AdditionalForceUI.SetAdditionalForceText(_AdditionalForce);

        // UI Image 표시 ( 방향 Index를 매개변수로 넘겨서 )
        _AdditionalForceUI.SetAdditionalForceImage(tRandomInt);
    }
}
