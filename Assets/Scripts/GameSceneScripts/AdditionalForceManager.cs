using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalForceManager : MonoBehaviour
{
    [SerializeField]
    AdditionalForceUI _AdditionalForceUI;   // UI 

    [SerializeField]
    float _MaxRandomForceRange;   // ���� �� ����  

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

    // ����
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
        // ���� ���� ����
        int tRandomInt = Random.Range(0, _ForceDirection.Length);

        // �� ũ��
        float tRandomfloat = 0;

        // ������ 0 �̸� �ܺ��� X
        if (tRandomInt != 0)
        {
            // ���� �� ũ�� ����
            tRandomfloat = Random.Range(_MinAdditionalForce, _MaxRandomForceRange);
            // �Ҽ��� 1�ڸ����� ����� ����
            _AdditionalForce = Mathf.Floor(tRandomfloat * 10f) / 10f;
        }     
        else
        {
            _AdditionalForce = tRandomfloat;
        }

        // ���� * ũ�� ���ͷ� ��ȯ
        _AdditonalForceVector = _AdditionalForce * _ForceDirection[tRandomInt];

        // UI Text ǥ��
        _AdditionalForceUI.SetAdditionalForceText(_AdditionalForce);

        // UI Image ǥ�� ( ���� Index�� �Ű������� �Ѱܼ� )
        _AdditionalForceUI.SetAdditionalForceImage(tRandomInt);
    }
}
