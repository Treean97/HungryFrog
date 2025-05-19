using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleLoginManager : MonoBehaviour
{


    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            SignIn();
        }
        else
        {
            Debug.Log("이미 로그인됨: " + PlayGamesPlatform.Instance.localUser.userName);
        }
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string tName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string tId = PlayGamesPlatform.Instance.GetUserId();
            string tImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            Debug.Log($"GPGS 로그인 성공: {tName} / {tId} / {tImgUrl}");
        }
        else
        {
            Debug.LogWarning("GPGS 로그인 실패");
        }
    }
}
