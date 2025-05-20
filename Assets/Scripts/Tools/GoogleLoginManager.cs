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
            SignIn();
        else
            ProcessAuthentication(SignInStatus.Success);
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // ���� �α��� ����
            string tName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string tId = PlayGamesPlatform.Instance.GetUserId();
            string tImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            Debug.Log($"GPGS �α��� ����: {tName} / {tId} / {tImgUrl}");

            // tName �ִ� 10���� ����
            if (tName.Length > 10)
                tName = tName.Substring(0, 10);

            // PlayFab�� ���� ID�� �α���
            PlayFabLeaderboardManager._Inst.LoginWithCustomId(tId);

            // DisplayId �� ��tName#����4�ڸ��� ����
            int suffix = Random.Range(1000, 10000);  // 1000~9999
            PlayFabLeaderboardManager._Inst.DisplayId = $"{tName}#{suffix}";

            // UI ����
            var ui = FindAnyObjectByType<MainSceneUIManager>();
            ui?.UpdateUI();

        }
        else
        {
            // ���� �α��� ���� �� ����̽� ID ������� fallback
            Debug.LogWarning("GPGS �α��� ����: " + status);

            // 1) ����̽� ���� ID �����ͼ� �ִ� 10���� �ڸ���
            string fallbackId = SystemInfo.deviceUniqueIdentifier;
            if (fallbackId.Length > 10)
                fallbackId = fallbackId.Substring(0, 10);

            // 2) PlayFab�� ����̽� ID�� �α���
            PlayFabLeaderboardManager._Inst.LoginWithCustomId(fallbackId);

            // 3) DisplayId �� ��fallbackId#����4�ڸ��� ����
            int suffix = Random.Range(1000, 10000);
            PlayFabLeaderboardManager._Inst.DisplayId = $"{fallbackId}#{suffix}";

            // 4) UI ����
            var ui = FindAnyObjectByType<MainSceneUIManager>();
            ui?.UpdateUI();
        }
    }
}
