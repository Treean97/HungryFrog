using System;
using System.Collections;
using PlayFab.ClientModels;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField]
    Vector3 _UIWaitingPosition;

    private Coroutine _CurrentCoroutine;

    private bool _UIIsRunning = false;
    public bool GetUIIsRunning => _UIIsRunning;

    [Header("Setting UI")]
    [SerializeField]
    GameObject _SettingUI;

    [SerializeField]
    float _MoveDuration;

    [Header("Setting UI Panel")]
    [SerializeField]
    GameObject[] _SettingUIPanels;

    [Header("UI Panel Info UI Set")]
    [SerializeField]
    TMP_Text _VersionText;

    [Header("LeaderBoard UI")]
    [SerializeField]
    GameObject _LeaderBoardUI;

    [Header("ChangeID UI")]
    [SerializeField]
    GameObject _ChangeIDUI;

    [SerializeField]
    TMP_InputField _IDInputField;

    [Header("DisplayID UI")]
    [SerializeField]
    GameObject _DisplayIDUI;

    [SerializeField]
    TMP_Text _DisplayID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // UI 위치 초기화
        ResetUIPosition();

        // Setting UI 초기화 
        SettingUIPanelSound();

        // InfoUI 초기화
        SetInfoUI();

        // ID 갱신
        UpdateUI();

        // UI 활성화
        SetActiveUI();
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

    // 위치 초기화
    void ResetUIPosition()
    {        
        _SettingUI.transform.localPosition = _UIWaitingPosition;
        _LeaderBoardUI.transform.localPosition = _UIWaitingPosition;
        _ChangeIDUI.transform.localPosition = _UIWaitingPosition;
    }

    void SetActiveUI()
    {
        _SettingUI.SetActive(true);
        _LeaderBoardUI.SetActive(true);
        _ChangeIDUI.SetActive(true);
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
            _CurrentCoroutine = StartCoroutine(MoveUICo(_SettingUI, Vector3.zero));

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
            _CurrentCoroutine = StartCoroutine(MoveUICo(_SettingUI, _UIWaitingPosition));

            // 사운드
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
        }
        
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


    // LeaderBoard UI
    public void LeaderBoardUIOn()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            _UIIsRunning = true;
            _CurrentCoroutine = StartCoroutine(MoveUICo(_LeaderBoardUI, Vector3.zero));

            // 사운드
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
        }

    }

    public void LeaderBoardOff()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            // 데이터 저장
            GameManager._Inst._DataSaveLoad.SaveData();

            _UIIsRunning = false;
            _CurrentCoroutine = StartCoroutine(MoveUICo(_LeaderBoardUI, _UIWaitingPosition));

            // 사운드
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
        }

    }


    // ChangeID UI
    public void ChangeIDUIOn()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        {
            _UIIsRunning = true;
            _CurrentCoroutine = StartCoroutine(MoveUICo(_ChangeIDUI, Vector3.zero));

            // 사운드
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
        }

    }

    public void ChangeIDOff()
    {
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);

        string tInputText = _IDInputField.text.Trim();



        if (string.IsNullOrEmpty(tInputText))
        {
            _UIIsRunning = false;
            _CurrentCoroutine = StartCoroutine(MoveUICo(_ChangeIDUI, _UIWaitingPosition));
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
            return;
        }

        // 무조건 처음부터 #숫자 붙이기
        System.Random rng = new System.Random();
        string randomSuffix = $"#{rng.Next(1000, 9999)}";
        string baseName = $"{tInputText}{randomSuffix}";

        Debug.Log($"최초 DisplayName 시도: {baseName}");

        TrySetDisplayNameWithHash(baseName, onSuccess: (finalName) =>
        {
            PlayFabLeaderboardManager._Inst.DisplayId = finalName;

            GameManager._Inst._DataSaveLoad._Data.DisplayId = finalName;

            UpdateUI();

            GameManager._Inst._DataSaveLoad.SaveData();

            _UIIsRunning = false;
            _CurrentCoroutine = StartCoroutine(MoveUICo(_ChangeIDUI, _UIWaitingPosition));
            SoundManager._Inst.PlaySFX(SoundCategory.SFX, "UIPop");
        },
        onError: (err) =>
        {
            Debug.LogError("DisplayName 설정 실패: " + err);
        });
    }


    void TrySetDisplayNameWithHash(string baseName, Action<string> onSuccess, Action<string> onError = null)
    {
        int tAttempt = 0;
        int tMaxAttempts = 5;
        System.Random tRandom = new System.Random();

        void Try(string tName)
        {
            var tRequest = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = tName
            };

            PlayFab.PlayFabClientAPI.UpdateUserTitleDisplayName(tRequest,
            result =>
            {
                Debug.Log($"DisplayName 설정 성공: {result.DisplayName}");
                onSuccess?.Invoke(result.DisplayName); // 최종 이름 전달
            },
            error =>
            {
                if (error.Error == PlayFabErrorCode.NameNotAvailable)
                {
                    if (tAttempt < tMaxAttempts)
                    {
                        tAttempt++;
                        string randomSuffix = $"#{tRandom.Next(1000, 9999)}";
                        string newName = $"{baseName}{randomSuffix}";
                        Debug.LogWarning($"중복으로 재시도: {newName}");
                        Try(newName);
                    }
                    else
                    {
                        onError?.Invoke("DisplayName 설정 실패 (중복 초과)");
                    }
                }
                else
                {
                    onError?.Invoke(error.GenerateErrorReport());
                }
            });
        }

        Try(baseName);
    }

    IEnumerator MoveUICo(GameObject tUI, Vector3 tTargetPos)
    {
        Vector3 tStartPos = tUI.transform.localPosition;
        float tElapsed = 0f;

        while (tElapsed < _MoveDuration)
        {
            tElapsed += Time.deltaTime;
            float tLerpT = Mathf.Clamp01(tElapsed / _MoveDuration);

            tUI.transform.localPosition = Vector3.Lerp(tStartPos, tTargetPos, tLerpT);
            yield return null;
        }

        tUI.transform.localPosition = tTargetPos;
        _CurrentCoroutine = null;
    }

    public void UpdateUI()
    {
        _DisplayID.text = $"ID : {PlayFabLeaderboardManager._Inst.DisplayId}";
    }
}
