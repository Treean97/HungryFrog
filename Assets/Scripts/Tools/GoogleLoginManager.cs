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
//                Debug.Log("GPGS �α��� ����: " + _PlayGamesID);
//                PlayFabLogin(_PlayGamesID); // �� PlayFab ����
//            }
//            else
//            {
//                Debug.LogError("GPGS �α��� ����");
//            }
//        });
//    }

//    void PlayFabLogin(string tGooglePlayId)
//    {
//        var request = new PlayFab.ClientModels.LoginWithGooglePlayGamesServicesRequest
//        {
//            TitleId = "YOUR_PLAYFAB_TITLE_ID", // PlayFab���� Ȯ���� Title ID
//            PlayerId = tGooglePlayId,
//            CreateAccount = true
//        };

//        PlayFab.PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, result =>
//        {
//            Debug.Log("PlayFab �α��� ����");
//        }, error =>
//        {
//            Debug.LogError("PlayFab �α��� ����: " + error.GenerateErrorReport());
//        });
//    }
//}
