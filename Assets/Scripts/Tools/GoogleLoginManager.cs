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
            // 구글 로그인 성공
            string tName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string tId = PlayGamesPlatform.Instance.GetUserId();
            string tImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            Debug.Log($"GPGS 로그인 성공: {tName} / {tId} / {tImgUrl}");

            // tName 최대 10글자 제한
            if (tName.Length > 10)
                tName = tName.Substring(0, 10);

            // PlayFab에 구글 ID로 로그인
            PlayFabLeaderboardManager._Inst.LoginWithCustomId(tId);

            // DisplayId 에 “tName#랜덤4자리” 설정
            int suffix = Random.Range(1000, 10000);  // 1000~9999
            PlayFabLeaderboardManager._Inst.DisplayId = $"{tName}#{suffix}";

            // UI 갱신
            var ui = FindAnyObjectByType<MainSceneUIManager>();
            ui?.UpdateUI();

        }
        else
        {
            // 구글 로그인 실패 → 디바이스 ID 기반으로 fallback
            Debug.LogWarning("GPGS 로그인 실패: " + status);

            // 1) 디바이스 고유 ID 가져와서 최대 10글자 자르기
            string fallbackId = SystemInfo.deviceUniqueIdentifier;
            if (fallbackId.Length > 10)
                fallbackId = fallbackId.Substring(0, 10);

            // 2) PlayFab에 디바이스 ID로 로그인
            PlayFabLeaderboardManager._Inst.LoginWithCustomId(fallbackId);

            // 3) DisplayId 에 “fallbackId#랜덤4자리” 설정
            int suffix = Random.Range(1000, 10000);
            PlayFabLeaderboardManager._Inst.DisplayId = $"{fallbackId}#{suffix}";

            // 4) UI 갱신
            var ui = FindAnyObjectByType<MainSceneUIManager>();
            ui?.UpdateUI();
        }
    }
}
