using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSceneLeaderBoardUIManager : MonoBehaviour
{
    [Header("LeaderBoard UI Setting")]
    [SerializeField] private GameObject _LeaderBoardEntryPrefab;
    [SerializeField] private Transform _ScrollViewContent;

    private readonly List<GameObject> _SpawnedEntries = new();

    public void ShowLeaderboard()
    {
        PlayFabLeaderboardManager._Inst.LoadLeaderboard(tList =>
        {
            RefreshLeaderboardUI(tList);
        },
        error =>
        {
            Debug.LogWarning("�������� �ҷ����� ����: " + error);
        });
    }

    private void RefreshLeaderboardUI(List<PlayerLeaderboardEntry> tEntries)
    {
        // ���� �׸� ����
        foreach (var obj in _SpawnedEntries)
            Destroy(obj);
        _SpawnedEntries.Clear();

        // �� �׸� ����
        foreach (var tEntry in tEntries)
        {
            GameObject tGO = Instantiate(_LeaderBoardEntryPrefab, _ScrollViewContent);
            TMP_Text[] tTexts = tGO.GetComponentsInChildren<TMP_Text>();
            tTexts[0].text = tEntry.DisplayName ?? "(No Name)";
            tTexts[1].text = tEntry.StatValue.ToString();
            _SpawnedEntries.Add(tGO);
        }
    }
}
