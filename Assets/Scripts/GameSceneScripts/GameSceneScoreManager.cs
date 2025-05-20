using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct AchievementInfo
{
    public int Threshold;      // ���� �Ӱ�ġ
    public string AchievementId;  // Play Console �� ��ϵ� ���� ID
    [NonSerialized] public bool Unlocked; // ���� ��� ����
}

public class GameSceneScoreManager : MonoBehaviour
{
    public static GameSceneScoreManager _Inst;

    [SerializeField] private int _ShootScoreMultiple;
    [SerializeField] private int _CombineScoreMultiple;
    [SerializeField] private TMP_Text[] _GameSceneScoreTextUI;

    // �Ӱ�ġ������ID �� ����Ʈ (�ν����Ϳ��� ���� ����)
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
        // ���� �迭�� ���� ���� ������������ ���� (������ ó�� ����)
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

            // ���� ������� �ʾҰ�, ������ �Ӱ�ġ �̻��̸� ���
            if (!info.Unlocked && _Score >= info.Threshold)
            {
                info.Unlocked = true;  // �ߺ� ����
                GPGSManager._Inst.UnlockAchievement(info.AchievementId);
            }
        }
    }
}
