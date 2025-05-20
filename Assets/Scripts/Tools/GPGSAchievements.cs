using System.Linq;
using GooglePlayGames;
using UnityEngine;

public class GPGSAchievements : MonoBehaviour
{
    public void ShowAchievementsBtn()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
        
    }
}
