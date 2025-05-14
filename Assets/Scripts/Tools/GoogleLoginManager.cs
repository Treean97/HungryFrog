//using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using System;

//public class GoogleLoginManager : MonoBehaviour
//{
//    public static GoogleLoginManager _Inst;

//    private void Awake()
//    {
//        if (_Inst == null)
//        {
//            _Inst = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        InitializeGPGS();
//    }

//    private void InitializeGPGS()
//    {
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//            .RequestServerAuthCode(false)  // true면 서버검증용, false는 간단하게 로그인만
//            .RequestIdToken()
//            .Build();

//        PlayGamesPlatform.InitializeInstance(config);
//        PlayGamesPlatform.Activate();

//        TryGoogleLogin();
//    }

//    private void TryGoogleLogin()
//    {
//        Social.localUser.Authenticate(success =>
//        {
//            if (success)
//            {
//                Debug.Log("GPGS 로그인 성공");

//                string idToken = PlayGamesPlatform.Instance.GetIdToken();

//                // PlayFab 로그인 시도 (Google ID 기반)
//                PlayFabLeaderboardManager._Inst.LoginWithGoogle(idToken);
//            }
//            else
//            {
//                Debug.LogWarning("GPGS 로그인 실패 → 커스텀 ID 사용");

//                // Fallback → CustomID 로그인
//                PlayFabLeaderboardManager._Inst.LoginWithCustomID();
//            }
//        });
//    }
//}
