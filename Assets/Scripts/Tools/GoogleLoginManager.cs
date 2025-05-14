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
//            .RequestServerAuthCode(false)  // true�� ����������, false�� �����ϰ� �α��θ�
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
//                Debug.Log("GPGS �α��� ����");

//                string idToken = PlayGamesPlatform.Instance.GetIdToken();

//                // PlayFab �α��� �õ� (Google ID ���)
//                PlayFabLeaderboardManager._Inst.LoginWithGoogle(idToken);
//            }
//            else
//            {
//                Debug.LogWarning("GPGS �α��� ���� �� Ŀ���� ID ���");

//                // Fallback �� CustomID �α���
//                PlayFabLeaderboardManager._Inst.LoginWithCustomID();
//            }
//        });
//    }
//}
