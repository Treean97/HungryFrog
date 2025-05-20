using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameSceneUIManager : MonoBehaviour
{
    [Header("Setting UI")]
    [SerializeField]
    GameObject _SettingUI;

    [SerializeField]
    Vector3 _UIWaitingPosition;

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


    [Header("Ending UI")]
    [SerializeField]
    GameObject _EndingUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 위치 초기화
        _SettingUI.transform.localPosition = _UIWaitingPosition;
        _EndingUI.transform.localPosition = _UIWaitingPosition;

        // 활성화
        SetActiveUI();

        // bool 초기화
        _UIIsRunning = false;

        // Setting UI 초기화 
        SettingUIPanelSound();

        // InfoUI 초기화
        SetInfoUI();
    }

    void SetActiveUI()
    {
        _SettingUI.SetActive(true);
        _EndingUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_UIIsRunning)
        {
            // 터치 사운드
            if (IsInputPressed())
            {
                SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UITouch");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ending();
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
        if (_UIIsRunning)
            return; // 이미 열려있으면 무시

        if (_CurrentCoroutine != null)
            StopCoroutine(_CurrentCoroutine);

        _CurrentCoroutine = StartCoroutine(MoveSettingUICo(Vector3.zero, true)); // true: 열기

        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
    }

    // UI 닫기
    public void SettingUIOff()
    {
        if (!_UIIsRunning)
            return; // 이미 닫혀있으면 무시

        if (_CurrentCoroutine != null)
            StopCoroutine(_CurrentCoroutine);

        _CurrentCoroutine = StartCoroutine(MoveSettingUICo(_UIWaitingPosition, false)); // false: 닫기

        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
    }

    // UI 이동 코루틴
    IEnumerator MoveSettingUICo(Vector3 tTargetPos, bool tIsOpening)
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

        _UIIsRunning = tIsOpening;
    }


    void SettingUIPanelOff()
    {
        foreach (var item in _SettingUIPanels)
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

    public void SettingUIPanelInfo()
    {
        SettingUIPanelOff();
        _SettingUIPanels[2].SetActive(true);
    }


    void SetInfoUI()
    {
        _VersionText.text = $"Version : {Application.version}";
    }

    public void Ending()
    {
        EndingUIPanelOn();
    }


    void EndingUIPanelOn()
    {        
        _CurrentCoroutine = StartCoroutine(MoveEndingUICo(Vector3.zero, true)); // true : 열기


        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
    }

    // UI 이동 코루틴
    IEnumerator MoveEndingUICo(Vector3 tTargetPos, bool tIsOpening)
    {
        Vector3 tStartPos = _EndingUI.transform.localPosition;
        float tElapsed = 0f;

        while (tElapsed < _MoveDuration)
        {
            tElapsed += Time.deltaTime;
            float tLerpT = Mathf.Clamp01(tElapsed / _MoveDuration);

            _EndingUI.transform.localPosition = Vector3.Lerp(tStartPos, tTargetPos, tLerpT);
            yield return null;
        }

        _EndingUI.transform.localPosition = tTargetPos;

        _CurrentCoroutine = null;

        _UIIsRunning = tIsOpening;
    }

    public void ReplayBtn()
    {
        // 로딩 씬 후 게임 씬 전환
        SceneManager.LoadScene("LoadingScene");
    }

    public void GoToMainBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
