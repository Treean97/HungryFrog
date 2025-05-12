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

        // ���̰� 25�ڸ� �ʰ��ϸ� �� 25���ڸ� ���
        if (id.Length > 25)
        {
            id = id.Substring(0, 25);
        }

        DisplayId = id;          // ������Ƽ�� �Ҵ��ϸ�, �α��� �� DisplayName���� ������
        LoginWithCustomID();     // PlayFab �α��� �� DisplayName ������Ʈ
                                 

        var tUI = FindFirstObjectByType<MainSceneDisplayIDUI>();
        if (tUI != null) tUI.UpdateUI();
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
            UpdateDisplayName(DisplayId); // �α��� ���� DisplayName ����
        },
        error =>
        {
            Debug.LogError("PlayFab �α��� ����: " + error.GenerateErrorReport());
        });
    }

    private void UpdateDisplayName(string newName)
    {
        // PlayFab �� ����� DisplayName ���� ��û
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
            Debug.LogWarning("DisplayName ���� ����: " + err.GenerateErrorReport());
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

    public void LoadScore(Action onSuccess = null, Action<string> onError = null)
    {
        // �α��� ���� Ȯ��
        if (!_isLoggedIn)
        {
            onError?.Invoke("Not logged in");
            return;
        }

        // HighScore ��� ��ȸ ��û
        var getReq = new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { "HighScore" }
        };

        PlayFabClientAPI.GetPlayerStatistics(getReq, res =>
        {
            var stat = res.Statistics.Find(s => s.StatisticName == "HighScore");
            _Score = stat != null ? stat.Value : 0;
            Debug.Log($"���� �ε� ����: {_Score}");
            onSuccess?.Invoke();
        },
        err =>
        {
            Debug.LogError("���� �ε� ����: " + err.GenerateErrorReport());
            onError?.Invoke(err.GenerateErrorReport());
        });
    }
}
