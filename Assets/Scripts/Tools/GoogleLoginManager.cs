//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

//public class GoogleLoginManager : MonoBehaviour
//{
//    void Start()
//    {
//        PlayGamesPlatform.DebugLogEnabled = true;

//        PlayGamesPlatform.Activate();

//        if (!PlayGamesPlatform.Instance.localUser.authenticated)
//            SignIn();
//        else
//            ProcessAuthentication(SignInStatus.Success);
//    }

//    public void SignIn()
//    {
//        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
//    }

//    internal void ProcessAuthentication(SignInStatus status)
//    {
//        if (status == SignInStatus.Success)
//        {
//            // 구글 로그인 성공
//            string tName = PlayGamesPlatform.Instance.GetUserDisplayName();
//            string tId = PlayGamesPlatform.Instance.GetUserId();
//            string tImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
//            Debug.Log($"GPGS 로그인 성공: {tName} / {tId} / {tImgUrl}");

//            // tName 최대 10글자 제한
//            if (tName.Length > 10)
//                tName = tName.Substring(0, 10);

//            // PlayFab에 구글 ID로 로그인
//            PlayFabLeaderboardManager._Inst.LoginWithCustomId(tId);

//            // DisplayId 에 “tName#랜덤4자리” 설정
//            int suffix = Random.Range(1000, 10000);  // 1000~9999
//            PlayFabLeaderboardManager._Inst.DisplayId = $"{tName}#{suffix}";

//            // UI 갱신
//            var ui = FindAnyObjectByType<MainSceneUIManager>();
//            ui?.UpdateUI();

//        }
//        else
//        {
//            // 구글 로그인 실패 → 디바이스 ID 기반으로 fallback
//            Debug.LogWarning("GPGS 로그인 실패: " + status);

//            // 1) 디바이스 고유 ID 가져와서 최대 10글자 자르기
//            string fallbackId = SystemInfo.deviceUniqueIdentifier;
//            if (fallbackId.Length > 10)
//                fallbackId = fallbackId.Substring(0, 10);

//            // 2) PlayFab에 디바이스 ID로 로그인
//            PlayFabLeaderboardManager._Inst.LoginWithCustomId(fallbackId);

//            // 3) DisplayId 에 “fallbackId#랜덤4자리” 설정
//            int suffix = Random.Range(1000, 10000);
//            PlayFabLeaderboardManager._Inst.DisplayId = $"{fallbackId}#{suffix}";

//            // 4) UI 갱신
//            var ui = FindAnyObjectByType<MainSceneUIManager>();
//            ui?.UpdateUI();
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleLoginManager : MonoBehaviour
{
    // ====================================================
    // 전역 필드: '_UpperCamelCase' 규칙
    // ====================================================
    private DataSaveLoad _DataSaveLoad;                     // JSON 읽고 쓸 때 사용하는 컴포넌트
    private PlayFabLeaderboardManager _PfManager;           // PlayFab 로그인 및 리더보드 관리 매니저
    private string _DeviceUID;                              // 디바이스 고유 ID

    void Start()
    {
        // ====================================================
        // 1. DataSaveLoad, PlayFabLeaderboardManager 인스턴스 찾기
        // ====================================================
        _DataSaveLoad = FindAnyObjectByType<DataSaveLoad>();
        _PfManager = PlayFabLeaderboardManager._Inst;

        if (_DataSaveLoad == null)
            Debug.LogError("DataSaveLoad 컴포넌트를 찾을 수 없습니다!");
        if (_PfManager == null)
            Debug.LogError("PlayFabLeaderboardManager 인스턴스를 찾을 수 없습니다!");

        // ====================================================
        // 2. 디바이스 고유 ID 가져오기
        // ====================================================
        _DeviceUID = SystemInfo.deviceUniqueIdentifier;

        // ====================================================
        // 3. GPGS 초기화 및 로그인 시도
        // ====================================================
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        else
        {
            // 이미 인증된 상태라면 직접 콜백 호출
            ProcessAuthentication(SignInStatus.Success);
        }
    }

    /// <summary>
    /// 구글 로그인 결과 callback
    /// </summary>
    internal void ProcessAuthentication(SignInStatus tStatus)
    {
        // ====================================================
        // 0. JSON에서 저장된 CustomID 로드
        // ====================================================
        _DataSaveLoad.LoadData();
        string tSavedID = _DataSaveLoad._Data.DisplayId;

        // “저장된 ID가 있으면서, 그것이 deviceUID와 다를 때”만 CustomID로 인정
        bool tHasSavedCustomID =
            !string.IsNullOrEmpty(tSavedID) && (tSavedID != _DeviceUID);

        if (tStatus == SignInStatus.Success)
        {
            // ====================================================
            // 1. 구글 로그인 성공
            // ====================================================
            Debug.Log("GPGS 로그인 성공!");

            if (tHasSavedCustomID)
            {
                // 1-1) 로컬에 CustomID가 있으면 → 그대로 사용
                Debug.Log($"로컬에 저장된 CustomID 사용 → {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 1-2) 로컬에 CustomID가 없으면 → 구글 ID를 CustomID로 사용
                string tGoogleID = PlayGamesPlatform.Instance.GetUserId();
                Debug.Log($"로컬에 저장된 ID 없음 → 구글 ID({tGoogleID})를 CustomID로 사용");

                // JSON에 저장
                _DataSaveLoad._Data.DisplayId = tGoogleID;
                _DataSaveLoad.SaveData();

                // PlayFab 로그인
                LoginToPlayFabWithId(tGoogleID);
            }
        }
        else
        {
            // ====================================================
            // 2. 구글 로그인 실패
            // ====================================================
            Debug.LogWarning($"GPGS 로그인 실패: {tStatus}");

            if (tHasSavedCustomID)
            {
                // 2-1) 로컬에 CustomID가 있으면 → 그대로 사용
                Debug.Log($"로컬에 저장된 CustomID 사용 → {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 2-2) 로컬에 CustomID가 없으면 → 랜덤 ID 생성
                int tRandom = Random.Range(1000, 10000); // 1000~9999
                string tRandomID = $"Guest_{tRandom}";
                Debug.Log($"로컬에 저장된 ID 없음 → 랜덤 CustomID 생성: {tRandomID}");

                // JSON에 저장
                _DataSaveLoad._Data.DisplayId = tRandomID;
                _DataSaveLoad.SaveData();

                // PlayFab 로그인
                LoginToPlayFabWithId(tRandomID);
            }
        }
    }


    private void LoginToPlayFabWithId(string tCustomId)
    {
        // 1) PlayFab에 CustomID로 로그인
        _PfManager.LoginWithCustomId(tCustomId);

        // 2) PlayFabLeaderboardManager 내부에서 로그인 성공 후에 DisplayName 동기화
        _PfManager.DisplayId = tCustomId;

        // UI 갱신
        var tUIManager = FindAnyObjectByType<MainSceneUIManager>();
        if (tUIManager != null)
        {
            tUIManager.UpdateUI();
        }
            
    }
}
