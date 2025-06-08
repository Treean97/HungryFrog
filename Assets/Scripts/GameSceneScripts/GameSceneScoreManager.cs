using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 1) 업적 정보 구조체
[Serializable]
public struct AchievementInfo
{
    public int Threshold;          // 달성 기준 점수
    public string AchievementId;   // Play Console에 등록된 업적 ID
    [NonSerialized] public bool Unlocked; // 업적 해제 여부 (런타임 전용)
}

public class GameSceneScoreManager : MonoBehaviour
{
    // 2) 싱글톤 인스턴스
    public static GameSceneScoreManager _Inst;

    // 3) 점수 가중치 설정
    [SerializeField] private int _ShootScoreMultiple;    // 발사 점수 배수
    [SerializeField] private int _CombineScoreMultiple;  // 합성 점수 배수

    // 4) 점수 표시용 UI 텍스트들
    [SerializeField] private TMP_Text[] _GameSceneScoreTextUI;

    // 5) 업적 정보 배열 (점수 기준별 업적)
    [SerializeField] private AchievementInfo[] _Achievements;

    // 6) 현재 점수
    private int _Score;
    public int GetScore => _Score;

    private void Awake()
    {
        // 7) 싱글톤 초기화
        if (_Inst == null) _Inst = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 8) 시작 시 점수 초기화 및 업적 배열 정렬
        _Score = 0;
        Array.Sort(_Achievements, (a, b) => a.Threshold.CompareTo(b.Threshold));
    }

    // 9) 투사체 발사로 점수 추가
    public void AddScoreByShoot(int tObjectID)
    {
        _Score += (tObjectID + 1) * _ShootScoreMultiple;
        UpdateScoreUI();
        CheckAchievements();
    }

    // 10) 아이템 합성으로 점수 추가
    public void AddScoreByCombine(int tObjectID)
    {
        _Score += (tObjectID + 1) * _CombineScoreMultiple;
        UpdateScoreUI();
        CheckAchievements();
    }

    // 11) UI 텍스트에 점수 갱신
    private void UpdateScoreUI()
    {
        foreach (var txt in _GameSceneScoreTextUI)
            txt.text = _Score.ToString();
    }

    // 12) 업적 달성 체크 및 해제
    private void CheckAchievements()
    {
        for (int i = 0; i < _Achievements.Length; i++)
        {
            ref var info = ref _Achievements[i];
            if (!info.Unlocked && _Score >= info.Threshold)
            {
                info.Unlocked = true;  
                GPGSManager._Inst.UnlockAchievement(info.AchievementId);
            }
        }
    }
}
