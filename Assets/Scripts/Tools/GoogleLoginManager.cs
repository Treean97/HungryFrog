using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleLoginManager : MonoBehaviour
{
    // 데이터 저장/로드 관리 클래스 참조
    private DataSaveLoad _DataSaveLoad;

    // PlayFab 리더보드 매니저 싱글톤 참조
    private PlayFabLeaderboardManager _PfManager;

    // 기기 고유 ID 저장용
    private string _DeviceUID;

    void Start()
    {
        // 1) 컴포넌트/매니저 참조 찾기
        _DataSaveLoad = FindAnyObjectByType<DataSaveLoad>();
        _PfManager     = PlayFabLeaderboardManager._Inst;

        if (_DataSaveLoad == null)
            Debug.LogError("DataSaveLoad 컴포넌트를 찾지 못했습니다!");
        if (_PfManager == null)
            Debug.LogError("PlayFabLeaderboardManager 인스턴스를 찾지 못했습니다!");

        // 2) 기기 고유 ID 가져오기
        _DeviceUID = SystemInfo.deviceUniqueIdentifier;

        // 3) GPGS (Google Play Games Services) 초기화
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        // 4) GPGS 로그인 시도 또는 이미 로그인 상태 처리
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        else
        {
            // 이미 로그인된 상태면 바로 처리
            ProcessAuthentication(SignInStatus.Success);
        }
    }

    /// GPGS 로그인 결과 콜백
    void ProcessAuthentication(SignInStatus tStatus)
    {
        // 1) 저장된 CustomID 불러오기
        _DataSaveLoad.LoadData();
        string tSavedID = _DataSaveLoad._Data.DisplayId;

        bool tHasSavedCustomID =
            !string.IsNullOrEmpty(tSavedID) && (tSavedID != _DeviceUID);

        if (tStatus == SignInStatus.Success)
        {
            Debug.Log("GPGS 로그인 성공!");

            if (tHasSavedCustomID)
            {
                // 1-1) 이전에 저장된 CustomID로 PlayFab 로그인
                Debug.Log($"저장된 CustomID 사용: {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 1-2) 구글 ID로 새 CustomID 생성 및 저장, PlayFab 로그인
                string tGoogleID = PlayGamesPlatform.Instance.GetUserId();
                Debug.Log($"Google ID 사용: {tGoogleID}");

                _DataSaveLoad._Data.DisplayId = tGoogleID;
                _DataSaveLoad.SaveData();

                LoginToPlayFabWithId(tGoogleID);
            }
        }
        else
        {
            Debug.LogWarning($"GPGS 로그인 실패: {tStatus}");

            if (tHasSavedCustomID)
            {
                // 2-1) 로그인 실패 시에도 저장된 CustomID로 PlayFab 로그인
                Debug.Log($"저장된 CustomID 재사용: {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 2-2) 비회원 랜덤 Guest ID 생성, 저장, PlayFab 로그인
                int    tRandom   = Random.Range(1000, 10000);
                string tRandomID = $"Guest_{tRandom}";
                Debug.Log($"랜덤 Guest ID 생성: {tRandomID}");

                _DataSaveLoad._Data.DisplayId = tRandomID;
                _DataSaveLoad.SaveData();

                LoginToPlayFabWithId(tRandomID);
            }
        }
    }

    /// PlayFab에 CustomID로 로그인하고, UI에 ID 표시까지 처리
    private void LoginToPlayFabWithId(string tCustomId)
    {
        // 1) PlayFab CustomID 로그인 호출
        _PfManager.LoginWithCustomId(tCustomId);

        // 2) PlayFabLeaderboardManager 에 DisplayId 설정
        _PfManager.DisplayId = tCustomId;

        // 3) 메인 UI에 로그인된 ID 업데이트
        var tUIManager = FindAnyObjectByType<MainSceneUIManager>();
        if (tUIManager != null)
        {
            tUIManager.UpdateUI();
        }
    }
}
