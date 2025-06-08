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
    GameObject _SettingUI;             // 설정 창 전체 오브젝트

    [SerializeField]
    Vector3 _UIWaitingPosition;        // UI 대기 위치

    [SerializeField]
    float _MoveDuration;               // UI 이동 시간

    private Coroutine _CurrentCoroutine; // 현재 실행 중인 코루틴

    private bool _UIIsRunning = false;   // UI 열려있는 중인지
    public bool GetUIIsRunning => _UIIsRunning;

    [Header("Setting UI Panel")]
    [SerializeField]
    GameObject[] _SettingUIPanels;     // 설정 패널들(탭별)

    [Header("UI Panel Info UI Set")]
    [SerializeField]
    TMP_Text _VersionText;             // 버전 정보 텍스트

    [Header("Ending UI")]
    [SerializeField]
    GameObject _EndingUI;              // 엔딩 UI 전체 오브젝트

    void Start()
    {
        // UI 위치 초기화
        _SettingUI.transform.localPosition = _UIWaitingPosition;
        _EndingUI.transform.localPosition  = _UIWaitingPosition;

        // UI 활성화(숨겨진 자식 활성화)
        SetActiveUI();

        _UIIsRunning = false;           // UI 상태 초기화

        // 설정 탭 기본값으로 "Sound" 선택
        SettingUIPanelSound();

        // 버전 정보 세팅
        SetInfoUI();
    }

    void SetActiveUI()
    {
        _SettingUI.SetActive(true);
        _EndingUI.SetActive(true);
    }

    void Update()
    {
        if (_UIIsRunning)
        {
            // UI 열려있는 상태에서 입력 감지 시 터치 사운드
            if (IsInputPressed())
                SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UITouch");
        }

        // 디버그용: 스페이스바 누르면 엔딩 UI 호출
        if (Input.GetKeyDown(KeyCode.Space))
            Ending();
    }

    // 마우스나 터치 입력이 시작되었는지 확인
    bool IsInputPressed()
    {
        return Input.GetMouseButtonDown(0) ||
               (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    // 설정 UI 열기 버튼 호출
    public void SettingUIOn()
    {
        if (_UIIsRunning) return;       // 이미 열려 있으면 무시
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);

        // 설정 창 열기 코루틴 실행
        _CurrentCoroutine = StartCoroutine(MoveSettingUICo(Vector3.zero, true));
        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
    }

    // 설정 UI 닫기 버튼 호출
    public void SettingUIOff()
    {
        if (!_UIIsRunning) return;      // 이미 닫혀 있으면 무시
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);

        // 설정 창 닫기 코루틴 실행
        _CurrentCoroutine = StartCoroutine(MoveSettingUICo(_UIWaitingPosition, false));
        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
    }

    // 설정 UI 이동 처리 코루틴
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
        _UIIsRunning = tIsOpening;     // 열림/닫힘 상태 반영
    }

    // 모든 설정 패널 비활성화
    void SettingUIPanelOff()
    {
        foreach (var item in _SettingUIPanels)
            item.SetActive(false);
    }

    // Sound 패널 선택
    public void SettingUIPanelSound()
    {
        SettingUIPanelOff();
        _SettingUIPanels[0].SetActive(true);
    }

    // Challenge 패널 선택
    public void SettingUIPanelChallenge()
    {
        SettingUIPanelOff();
        _SettingUIPanels[1].SetActive(true);
    }

    // Info 패널 선택
    public void SettingUIPanelInfo()
    {
        SettingUIPanelOff();
        _SettingUIPanels[2].SetActive(true);
    }

    // 버전 정보 텍스트 설정
    void SetInfoUI()
    {
        _VersionText.text = $"Version : {Application.version}";
    }

    // 엔딩 UI 열기
    public void Ending()
    {
        EndingUIPanelOn();
    }

    void EndingUIPanelOn()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        _CurrentCoroutine = StartCoroutine(MoveEndingUICo(Vector3.zero, true));
        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
    }

    // 엔딩 UI 이동 처리 코루틴
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

    // 리플레이 버튼 클릭 시 로딩 씬으로 이동
    public void ReplayBtn()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    // 메인 버튼 클릭 시 메인 씬으로 이동
    public void GoToMainBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
