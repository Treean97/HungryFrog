using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    [Header("Setting UI")]
    [SerializeField]
    GameObject _SettingUI;

    [SerializeField]
    Vector3 _SettingUIWaitingPosition;

    [SerializeField]
    float _MoveDuration;

    private Coroutine _CurrentCoroutine;

    private bool _UIIsRunning = false;
    public bool GetUIIsRunning => _UIIsRunning;

    [Header("Setting UI Panel")]
    [SerializeField]
    GameObject[] _SettingUIPanels;


    [Header("UI Panel Info UI Set")]
    [SerializeField]
    TMP_Text _VersionText;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 위치 초기화
        _SettingUI.transform.localPosition = _SettingUIWaitingPosition;

        // Setting UI 초기화 
        SettingUIPanelSound();

        // InfoUI 초기화
        SetInfoUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(_UIIsRunning)
        {
            // 터치 사운드
            if(IsInputPressed())
            {
                SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UITouch");
            }
        }
    }

    // 터치 및 클릭 감지
    bool IsInputPressed()
    {
        return Input.GetMouseButtonDown(0) ||
           (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    // Setting UI
    public void SettingUIOn()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            _UIIsRunning = true;
            _CurrentCoroutine = StartCoroutine(MoveSettingUI(Vector3.zero));

            // 사운드
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
        }
        
    }

    public void SettingUIOff()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            // 데이터 저장
            GameManager._Inst._DataSaveLoad.SaveData();

            _UIIsRunning = false;
            _CurrentCoroutine = StartCoroutine(MoveSettingUI(_SettingUIWaitingPosition));

            // 사운드
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
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

    void SettingUIPanelOff()
    {
        foreach(var item in _SettingUIPanels)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SettingUIPanelSound()
    {
        SettingUIPanelOff();
        _SettingUIPanels[0].SetActive(true);
    }

    public void SettingUIPanelChallenge()
    {
        SettingUIPanelOff();
        _SettingUIPanels[1].SetActive(true);
    }

    public void SettingUIInfo()
    {
        SettingUIPanelOff();
        _SettingUIPanels[2].SetActive(true);
    }

    void SetInfoUI()
    {
        _VersionText.text = $"Version : {Application.version}";
    }
}
