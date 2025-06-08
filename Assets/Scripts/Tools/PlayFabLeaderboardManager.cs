using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboardManager : MonoBehaviour
{
    // 1) 싱글톤 인스턴스 관리
    public static PlayFabLeaderboardManager _Inst;

    // 로그인 상태 플래그
    private bool _isLoggedIn = false;

    // 2) 표시할 ID 관리 프로퍼티
    private string _DisplayId;
    public string DisplayId
    {
        get => _DisplayId;
        set
        {
            _DisplayId = value;
            // 이미 로그인된 상태라면 PlayFab에 DisplayName 갱신 호출
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

    // 3) PlayFab Custom ID 로그인
    public void LoginWithCustomId(string customId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId      = customId,   // 로그인에 사용할 Custom ID
            CreateAccount = true        // 계정이 없으면 자동 생성
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                Debug.Log($"PlayFab 로그인 성공: {customId}");
                _isLoggedIn = true;

                // 로그인 이후 DisplayId가 변경되어야 할 경우 갱신
                if (!string.IsNullOrEmpty(_DisplayId) && _DisplayId != customId)
                    UpdateDisplayName(_DisplayId);
            },
            error =>
            {
                Debug.LogError($"PlayFab 로그인 실패: {error.GenerateErrorReport()}");
            });
    }

    // 4) PlayFab 사용자 타이틀 DisplayName 업데이트
    private void UpdateDisplayName(string newName)
    {
        var nameReq = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName    // 갱신할 DisplayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(nameReq,
            res =>
            {
                Debug.Log($"DisplayName 갱신 성공: {res.DisplayName}");
            },
            err =>
            {
                if (err.Error == PlayFabErrorCode.NameNotAvailable)
                    Debug.LogWarning($"이미 사용 중인 DisplayName: {newName}");
                else
                    Debug.LogWarning($"DisplayName 갱신 실패: {err.GenerateErrorReport()}");
            });
    }

    // 5) 점수 저장 요청
    public void SaveScore(int tScore, Action onSuccess = null, Action<string> onError = null)
    {
        // 로그인 확인
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
                    StatisticName = "HighScore",  // 저장할 통계 이름
                    Value         = tScore        // 저장할 점수 값
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

    // 6) 리더보드 데이터 불러오기
    public void LoadLeaderboard(Action<List<PlayerLeaderboardEntry>> onSuccess, Action<string> onError = null)
    {
        // 로그인 확인
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName    = "HighScore",  // 조회할 통계 이름
            StartPosition    = 0,            // 시작 순위
            MaxResultsCount  = 30            // 최대 결과 개수
        };

        PlayFabClientAPI.GetLeaderboard(request,
            result =>
            {
                // 콜백으로 리더보드 리스트 반환
                onSuccess?.Invoke(result.Leaderboard);
            },
            error =>
            {
                Debug.LogError($"리더보드 불러오기 실패: {error.GenerateErrorReport()}");
                onError?.Invoke(error.GenerateErrorReport());
            });
    }
}
