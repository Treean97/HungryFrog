using GooglePlayGames;
using UnityEngine;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager _Inst { get; private set; }

    void Awake()
    {
        if (_Inst == null)
        {
            _Inst = this; DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 업적 해제
    public void UnlockAchievement(string achievementId)
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
            PlayGamesPlatform.Instance.ReportProgress(achievementId, 100.0, success => {
                Debug.Log($"Unlock {achievementId}: {success}");
            });
    }
}
