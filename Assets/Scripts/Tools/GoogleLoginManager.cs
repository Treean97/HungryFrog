//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using UnityEngine;

//public class GoogleLoginManager : MonoBehaviour
//{
//    public static string _PlayGamesID;

//    void Start()
//    {
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//            .RequestServerAuthCode(false)
//            .RequestEmail()
//            .RequestIdToken()
//            .Build();

//        PlayGamesPlatform.InitializeInstance(config);
//        PlayGamesPlatform.Activate();

//        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
//        {
//            if (result == SignInStatus.Success)
//            {
//                _PlayGamesID = Social.localUser.id;
//                Debug.Log("GPGS 로그인 성공: " + _PlayGamesID);
//                PlayFabLogin(_PlayGamesID); // → PlayFab 연동
//            }
//            else
//            {
//                Debug.LogError("GPGS 로그인 실패");
//            }
//        });
//    }

//    void PlayFabLogin(string tGooglePlayId)
//    {
//        var request = new PlayFab.ClientModels.LoginWithGooglePlayGamesServicesRequest
//        {
//            TitleId = "YOUR_PLAYFAB_TITLE_ID", // PlayFab에서 확인한 Title ID
//            PlayerId = tGooglePlayId,
//            CreateAccount = true
//        };

//        PlayFab.PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, result =>
//        {
//            Debug.Log("PlayFab 로그인 성공");
//        }, error =>
//        {
//            Debug.LogError("PlayFab 로그인 실패: " + error.GenerateErrorReport());
//        });
//    }
//}
