using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSceneLeaderBoardUIManager : MonoBehaviour
{
    [Header("리더보드 UI 설정")]
    [SerializeField] private GameObject _LeaderBoardEntryPrefab; // 엔트리 프리팹
    [SerializeField] private Transform _ScrollViewContent;      // 스크롤뷰 Content 영역

    private readonly List<GameObject> _SpawnedEntries = new();  // 생성된 엔트리 리스트

    // 1) 리더보드 데이터 로드 후 UI 갱신
    public void ShowLeaderboard()
    {
        PlayFabLeaderboardManager._Inst.LoadLeaderboard(tList =>
        {
            RefreshLeaderboardUI(tList);
        },
        error =>
        {
            Debug.LogWarning("리더보드 로드 실패: " + error);
        });
    }

    // 2) 기존 엔트리 제거 & 새로운 엔트리 생성
    private void RefreshLeaderboardUI(List<PlayerLeaderboardEntry> tEntries)
    {
        // 2-1) 이전에 생성된 엔트리 삭제
        foreach (var obj in _SpawnedEntries)
            Destroy(obj);
        _SpawnedEntries.Clear();

        // 2-2) 새 엔트리마다 프리팹 인스턴스화 및 텍스트 세팅
        foreach (var tEntry in tEntries)
        {
            GameObject tGO = Instantiate(_LeaderBoardEntryPrefab, _ScrollViewContent);
            TMP_Text[] tTexts = tGO.GetComponentsInChildren<TMP_Text>();
            tTexts[0].text = tEntry.DisplayName ?? "(No Name)"; // 이름
            tTexts[1].text = tEntry.StatValue.ToString();       // 점수
            _SpawnedEntries.Add(tGO);
        }
    }
}
