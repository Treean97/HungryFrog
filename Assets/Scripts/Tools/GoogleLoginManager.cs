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
            Debug.Log("�̹� �α��ε�: " + PlayGamesPlatform.Instance.localUser.userName);
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

            Debug.Log($"GPGS �α��� ����: {tName} / {tId} / {tImgUrl}");
        }
        else
        {
            Debug.LogWarning("GPGS �α��� ����");
        }
    }
}
