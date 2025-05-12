// PlayFabLeaderboardManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboardManager : MonoBehaviour
{
    public static PlayFabLeaderboardManager _Inst; // 싱글톤 인스턴스
    private bool _isLoggedIn = false;                // 로그인 상태 플래그

    // 점수 관리 필드와 프로퍼티
    private int _Score = 0;
    public int Score
    {
        get => _Score;
        set => _Score = value;
    }

    // 표시 ID 관리 필드와 프로퍼티
    private string _DisplayId;
    public string DisplayId
    {
        get => _DisplayId;
        set
        {
            _DisplayId = value;
            if (_isLoggedIn)
                UpdateDisplayName(_DisplayId); // 로그인 후 이름 갱신
        }
    }

    private void Awake()
    {
        // 싱글톤 설정
        if (_Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // DataSaveLoad에서 로드한 DisplayId 가져오기
        var tDataLoader = GameManager._Inst.GetComponent<DataSaveLoad>();
        string id = (tDataLoader != null && !string.IsNullOrEmpty(tDataLoader._Data.DisplayId))
            ? tDataLoader._Data.DisplayId
            : SystemInfo.deviceUniqueIdentifier;

        // 길이가 25자를 초과하면 앞 25글자만 사용
        if (id.Length > 25)
        {
            id = id.Substring(0, 25);
        }

        DisplayId = id;          // 프로퍼티에 할당하며, 로그인 후 DisplayName으로 설정됨
        LoginWithCustomID();     // PlayFab 로그인 및 DisplayName 업데이트
                                 

        var tUI = FindFirstObjectByType<MainSceneDisplayIDUI>();
        if (tUI != null) tUI.UpdateUI();
    }

    public void Init()
    {

    }

    public void LoginWithCustomID()
    {
        // 디바이스 고유 ID로 익명 로그인 요청
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("PlayFab 로그인 성공");
            _isLoggedIn = true;
            UpdateDisplayName(DisplayId); // 로그인 직후 DisplayName 설정
        },
        error =>
        {
            Debug.LogError("PlayFab 로그인 실패: " + error.GenerateErrorReport());
        });
    }

    private void UpdateDisplayName(string newName)
    {
        // PlayFab 내 사용자 DisplayName 갱신 요청
        var nameReq = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(nameReq, res =>
        {
            Debug.Log("DisplayName 설정 성공: " + res.DisplayName);
        },
        err =>
        {
            Debug.LogWarning("DisplayName 설정 실패: " + err.GenerateErrorReport());
        });
    }

    public void SaveScore(int tScore, Action onSuccess = null, Action<string> onError = null)
    {
        // 로그인 상태 확인
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

        // HighScore 통계로 점수 저장 요청
        var statReq = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value         = tScore
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(statReq, res =>
        {
            Debug.Log($"점수 저장 성공: {tScore}");
            onSuccess?.Invoke();
        },
        err =>
        {
            Debug.LogError("점수 저장 실패: " + err.GenerateErrorReport());
            onError?.Invoke(err.GenerateErrorReport());
        });
    }

    public void LoadScore(Action onSuccess = null, Action<string> onError = null)
    {
        // 로그인 상태 확인
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

        // HighScore 통계 조회 요청
        var getReq = new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { "HighScore" }
        };

        PlayFabClientAPI.GetPlayerStatistics(getReq, res =>
        {
            var stat = res.Statistics.Find(s => s.StatisticName == "HighScore");
            _Score = stat != null ? stat.Value : 0;
            Debug.Log($"점수 로드 성공: {_Score}");
            onSuccess?.Invoke();
        },
        err =>
        {
            Debug.LogError("점수 로드 실패: " + err.GenerateErrorReport());
            onError?.Invoke(err.GenerateErrorReport());
        });
    }
}
