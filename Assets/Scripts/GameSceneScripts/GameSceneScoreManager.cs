using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct AchievementInfo
{
    public int Threshold;      // 점수 임계치
    public string AchievementId;  // Play Console 에 등록된 업적 ID
    [NonSerialized] public bool Unlocked; // 로컬 언락 상태
}

public class GameSceneScoreManager : MonoBehaviour
{
    public static GameSceneScoreManager _Inst;

    [SerializeField] private int _ShootScoreMultiple;
    [SerializeField] private int _CombineScoreMultiple;
    [SerializeField] private TMP_Text[] _GameSceneScoreTextUI;

    // 임계치·업적ID 쌍 리스트 (인스펙터에서 설정 가능)
    [SerializeField] private AchievementInfo[] _Achievements;

    private int _Score;
    public int GetScore => _Score;

    private void Awake()
    {
        if (_Inst == null) _Inst = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _Score = 0;
        // 업적 배열을 점수 기준 오름차순으로 정렬 (안정적 처리 위해)
        Array.Sort(_Achievements, (a, b) => a.Threshold.CompareTo(b.Threshold));
    }

    public void AddScoreByShoot(int tObjectID)
    {
        _Score += (tObjectID + 1) * _ShootScoreMultiple;
        UpdateScoreUI();
        CheckAchievements();
    }

    public void AddScoreByCombine(int tObjectID)
    {
        _Score += (tObjectID + 1) * _CombineScoreMultiple;
        UpdateScoreUI();
        CheckAchievements();
    }

    private void UpdateScoreUI()
    {
        foreach (var txt in _GameSceneScoreTextUI)
            txt.text = _Score.ToString();
    }

    private void CheckAchievements()
    {
        for (int i = 0; i < _Achievements.Length; i++)
        {
            ref var info = ref _Achievements[i];

            // 아직 언락되지 않았고, 점수가 임계치 이상이면 언락
            if (!info.Unlocked && _Score >= info.Threshold)
            {
                info.Unlocked = true;  // 중복 방지
                GPGSManager._Inst.UnlockAchievement(info.AchievementId);
            }
        }
    }
}
