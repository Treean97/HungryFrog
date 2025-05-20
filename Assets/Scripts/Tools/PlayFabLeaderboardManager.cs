using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboardManager : MonoBehaviour
{
    public static PlayFabLeaderboardManager _Inst;
    private bool _isLoggedIn = false;            // �α��� ���� �÷���

    // ǥ�� ID ���� �ʵ� �� ������Ƽ
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

    // PlayFab�� Custom ID(���⼭�� GoogleId)�� �α���
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
                Debug.Log($"PlayFab �α��� ����: {customId}");
                _isLoggedIn = true;

                // �α��� ��, DisplayId�� �̹� �����Ǿ� ������ PlayFab �����ʿ��� ����ȭ
                if (!string.IsNullOrEmpty(_DisplayId) && _DisplayId != customId)
                    UpdateDisplayName(_DisplayId);
            },
            error =>
            {
                Debug.LogError($"PlayFab �α��� ����: {error.GenerateErrorReport()}");
            });
    }

    // PlayFab �������� DisplayName ������Ʈ
    private void UpdateDisplayName(string newName)
    {
        var nameReq = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(nameReq,
            res =>
            {
                Debug.Log($"DisplayName ���� ����: {res.DisplayName}");
            },
            err =>
            {
                if (err.Error == PlayFabErrorCode.NameNotAvailable)
                    Debug.LogWarning($"�̹� ��� ���� DisplayName: {newName}");
                else
                    Debug.LogWarning($"DisplayName ���� ����: {err.GenerateErrorReport()}");
            });
    }

    // ���� ����
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
                Debug.Log($"���� ���� ����: {tScore}");
                onSuccess?.Invoke();
            },
            err =>
            {
                Debug.LogError($"���� ���� ����: {err.GenerateErrorReport()}");
                onError?.Invoke(err.GenerateErrorReport());
            });
    }

    // �������� �ε�
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
                Debug.LogError($"�������� �ε� ����: {error.GenerateErrorReport()}");
                onError?.Invoke(error.GenerateErrorReport());
            });
    }
}
