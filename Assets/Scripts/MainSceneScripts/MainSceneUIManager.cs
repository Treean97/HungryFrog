using System.Collections;
using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _SettingUI;

    [SerializeField]
    Vector3 _SettingUIWaitingPosition;

    [SerializeField]
    float _MoveDuration;

    private Coroutine _CurrentCoroutine;

    private bool _UIIsRunning = false;
    public bool GetUIIsRunning => _UIIsRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 위치 초기화
        _SettingUI.transform.localPosition = _SettingUIWaitingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Setting UI
    public void SettingUIOn()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            _UIIsRunning = true;
            _CurrentCoroutine = StartCoroutine(MoveSettingUI(Vector3.zero));
        }
        
    }

    public void SettingUIOff()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            _UIIsRunning = false;
            _CurrentCoroutine = StartCoroutine(MoveSettingUI(_SettingUIWaitingPosition));
        }
        
    }

    IEnumerator MoveSettingUI(Vector3 tTargetPos)
    {
        Vector3 tStartPos = _SettingUI.transform.localPosition;
        float tElapsed = 0f;

        while (tElapsed < _MoveDuration)
        {
            tElapsed += Time.deltaTime;
            float tLerpT = Mathf.Clamp01(tElapsed / _MoveDuration);

            _SettingUI.transform.localPosition = Vector3.Lerp(tStartPos, tTargetPos, tLerpT);
            yield return null;
        }

        _SettingUI.transform.localPosition = tTargetPos;
        _CurrentCoroutine = null;
    }


}
