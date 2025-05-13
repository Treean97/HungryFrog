// PlayFabLeaderboardManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboardManager : MonoBehaviour
{
    public static PlayFabLeaderboardManager _Inst; // �̱��� �ν��Ͻ�
    private bool _isLoggedIn = false;                // �α��� ���� �÷���

    // ���� ���� �ʵ�� ������Ƽ
    private int _Score = 0;
    public int Score
    {
        get => _Score;
        set => _Score = value;
    }

    // ǥ�� ID ���� �ʵ�� ������Ƽ
    private string _DisplayId;
    public string DisplayId
    {
        get => _DisplayId;
        set
        {
            _DisplayId = value;
            if (_isLoggedIn)
                UpdateDisplayName(_DisplayId); // �α��� �� �̸� ����
        }
    }

    private void Awake()
    {
        // �̱��� ����
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
        // DataSaveLoad���� �ε��� DisplayId ��������
        var tDataLoader = GameManager._Inst.GetComponent<DataSaveLoad>();
        string id = (tDataLoader != null && !string.IsNullOrEmpty(tDataLoader._Data.DisplayId))
            ? tDataLoader._Data.DisplayId
            : SystemInfo.deviceUniqueIdentifier;

        // �ִ� ���� �� ����
        if (id.Length > 15)
        {
            id = id.Substring(0, 15);
        }

        DisplayId = id;          // ������Ƽ�� �Ҵ��ϸ�, �α��� �� DisplayName���� ������
        LoginWithCustomID();     // PlayFab �α��� �� DisplayName ������Ʈ
                                 

        var tUI = FindFirstObjectByType<MainSceneUIManager>();
        if (tUI != null)
        {
            tUI.UpdateUI();
        }
    }

    public void Init()
    {

    }

    public void LoginWithCustomID()
    {
        // ����̽� ���� ID�� �͸� �α��� ��û
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("PlayFab �α��� ����");
            _isLoggedIn = true;

            // �α����� ������� DisplayName Ȯ��
            var tDisplayName = result.InfoResultPayload?.PlayerProfile?.DisplayName;

            // �̹� ���� �̸��̸� ����, �ٸ��� ����
            if (tDisplayName != DisplayId)
            {
                UpdateDisplayName(DisplayId);
            }
        },
        error =>
        {
            Debug.LogError("PlayFab �α��� ����: " + error.GenerateErrorReport());
        });
    }

    private void UpdateDisplayName(string newName)
    {
        var nameReq = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(nameReq, res =>
        {
            Debug.Log("DisplayName ���� ����: " + res.DisplayName);
        },
        err =>
        {
            if (err.Error == PlayFabErrorCode.NameNotAvailable)
            {
                Debug.LogWarning($"�̹� ��� ���� DisplayName: {newName}");
                // ���� ���ڿ� �ٿ��� ��õ��� �� ����
            }
            else
            {
                Debug.LogWarning("DisplayName ���� ����: " + err.GenerateErrorReport());
            }
        });
    }

    public void SaveScore(int tScore, Action onSuccess = null, Action<string> onError = null)
    {
        // �α��� ���� Ȯ��
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

        // HighScore ���� ���� ���� ��û
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
            Debug.Log($"���� ���� ����: {tScore}");
            onSuccess?.Invoke();
        },
        err =>
        {
            Debug.LogError("���� ���� ����: " + err.GenerateErrorReport());
            onError?.Invoke(err.GenerateErrorReport());
        });
    }


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

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            onSuccess?.Invoke(result.Leaderboard);
        }, error =>
        {
            Debug.LogError("�������� �ε� ����: " + error.GenerateErrorReport());
            onError?.Invoke(error.GenerateErrorReport());
        });
    }
}
