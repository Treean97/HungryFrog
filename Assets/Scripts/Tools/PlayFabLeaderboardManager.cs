using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboardManager : MonoBehaviour
{
    public static PlayFabLeaderboardManager _Inst;
    private bool _isLoggedIn = false;            // 로그인 상태 플래그

    // 표시 ID 관리 필드 및 프로퍼티
    private string _DisplayId;
    public string DisplayId
    {
        get => _DisplayId;
        set
        {
            _DisplayId = value;
            if (_isLoggedIn)
                UpdateDisplayName(_DisplayId);
        }
    }

    private void Awake()
    {
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

    // PlayFab에 Custom ID(여기서는 GoogleId)로 로그인
    public void LoginWithCustomId(string customId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                Debug.Log($"PlayFab 로그인 성공: {customId}");
                _isLoggedIn = true;

                // 로그인 후, DisplayId가 이미 설정되어 있으면 PlayFab 프로필에도 동기화
                if (!string.IsNullOrEmpty(_DisplayId) && _DisplayId != customId)
                    UpdateDisplayName(_DisplayId);
            },
            error =>
            {
                Debug.LogError($"PlayFab 로그인 실패: {error.GenerateErrorReport()}");
            });
    }

    // PlayFab 프로필의 DisplayName 업데이트
    private void UpdateDisplayName(string newName)
    {
        var nameReq = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(nameReq,
            res =>
            {
                Debug.Log($"DisplayName 설정 성공: {res.DisplayName}");
            },
            err =>
            {
                if (err.Error == PlayFabErrorCode.NameNotAvailable)
                    Debug.LogWarning($"이미 사용 중인 DisplayName: {newName}");
                else
                    Debug.LogWarning($"DisplayName 설정 실패: {err.GenerateErrorReport()}");
            });
    }

    // 점수 저장
    public void SaveScore(int tScore, Action onSuccess = null, Action<string> onError = null)
    {
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

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

        PlayFabClientAPI.UpdatePlayerStatistics(statReq,
            res =>
            {
                Debug.Log($"점수 저장 성공: {tScore}");
                onSuccess?.Invoke();
            },
            err =>
            {
                Debug.LogError($"점수 저장 실패: {err.GenerateErrorReport()}");
                onError?.Invoke(err.GenerateErrorReport());
            });
    }

    // 리더보드 로드
    public void LoadLeaderboard(Action<List<PlayerLeaderboardEntry>> onSuccess, Action<string> onError = null)
    {
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 30
        };

        PlayFabClientAPI.GetLeaderboard(request,
            result =>
            {
                onSuccess?.Invoke(result.Leaderboard);
            },
            error =>
            {
                Debug.LogError($"리더보드 로딩 실패: {error.GenerateErrorReport()}");
                onError?.Invoke(error.GenerateErrorReport());
            });
    }
}
