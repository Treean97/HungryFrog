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
//            // ���� �α��� ����
//            string tName = PlayGamesPlatform.Instance.GetUserDisplayName();
//            string tId = PlayGamesPlatform.Instance.GetUserId();
//            string tImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
//            Debug.Log($"GPGS �α��� ����: {tName} / {tId} / {tImgUrl}");

//            // tName �ִ� 10���� ����
//            if (tName.Length > 10)
//                tName = tName.Substring(0, 10);

//            // PlayFab�� ���� ID�� �α���
//            PlayFabLeaderboardManager._Inst.LoginWithCustomId(tId);

//            // DisplayId �� ��tName#����4�ڸ��� ����
//            int suffix = Random.Range(1000, 10000);  // 1000~9999
//            PlayFabLeaderboardManager._Inst.DisplayId = $"{tName}#{suffix}";

//            // UI ����
//            var ui = FindAnyObjectByType<MainSceneUIManager>();
//            ui?.UpdateUI();

//        }
//        else
//        {
//            // ���� �α��� ���� �� ����̽� ID ������� fallback
//            Debug.LogWarning("GPGS �α��� ����: " + status);

//            // 1) ����̽� ���� ID �����ͼ� �ִ� 10���� �ڸ���
//            string fallbackId = SystemInfo.deviceUniqueIdentifier;
//            if (fallbackId.Length > 10)
//                fallbackId = fallbackId.Substring(0, 10);

//            // 2) PlayFab�� ����̽� ID�� �α���
//            PlayFabLeaderboardManager._Inst.LoginWithCustomId(fallbackId);

//            // 3) DisplayId �� ��fallbackId#����4�ڸ��� ����
//            int suffix = Random.Range(1000, 10000);
//            PlayFabLeaderboardManager._Inst.DisplayId = $"{fallbackId}#{suffix}";

//            // 4) UI ����
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
    // ���� �ʵ�: '_UpperCamelCase' ��Ģ
    // ====================================================
    private DataSaveLoad _DataSaveLoad;                     // JSON �а� �� �� ����ϴ� ������Ʈ
    private PlayFabLeaderboardManager _PfManager;           // PlayFab �α��� �� �������� ���� �Ŵ���
    private string _DeviceUID;                              // ����̽� ���� ID

    void Start()
    {
        // ====================================================
        // 1. DataSaveLoad, PlayFabLeaderboardManager �ν��Ͻ� ã��
        // ====================================================
        _DataSaveLoad = FindAnyObjectByType<DataSaveLoad>();
        _PfManager = PlayFabLeaderboardManager._Inst;

        if (_DataSaveLoad == null)
            Debug.LogError("DataSaveLoad ������Ʈ�� ã�� �� �����ϴ�!");
        if (_PfManager == null)
            Debug.LogError("PlayFabLeaderboardManager �ν��Ͻ��� ã�� �� �����ϴ�!");

        // ====================================================
        // 2. ����̽� ���� ID ��������
        // ====================================================
        _DeviceUID = SystemInfo.deviceUniqueIdentifier;

        // ====================================================
        // 3. GPGS �ʱ�ȭ �� �α��� �õ�
        // ====================================================
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        else
        {
            // �̹� ������ ���¶�� ���� �ݹ� ȣ��
            ProcessAuthentication(SignInStatus.Success);
        }
    }

    /// <summary>
    /// ���� �α��� ��� callback
    /// </summary>
    internal void ProcessAuthentication(SignInStatus tStatus)
    {
        // ====================================================
        // 0. JSON���� ����� CustomID �ε�
        // ====================================================
        _DataSaveLoad.LoadData();
        string tSavedID = _DataSaveLoad._Data.DisplayId;

        // ������� ID�� �����鼭, �װ��� deviceUID�� �ٸ� ������ CustomID�� ����
        bool tHasSavedCustomID =
            !string.IsNullOrEmpty(tSavedID) && (tSavedID != _DeviceUID);

        if (tStatus == SignInStatus.Success)
        {
            // ====================================================
            // 1. ���� �α��� ����
            // ====================================================
            Debug.Log("GPGS �α��� ����!");

            if (tHasSavedCustomID)
            {
                // 1-1) ���ÿ� CustomID�� ������ �� �״�� ���
                Debug.Log($"���ÿ� ����� CustomID ��� �� {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 1-2) ���ÿ� CustomID�� ������ �� ���� ID�� CustomID�� ���
                string tGoogleID = PlayGamesPlatform.Instance.GetUserId();
                Debug.Log($"���ÿ� ����� ID ���� �� ���� ID({tGoogleID})�� CustomID�� ���");

                // JSON�� ����
                _DataSaveLoad._Data.DisplayId = tGoogleID;
                _DataSaveLoad.SaveData();

                // PlayFab �α���
                LoginToPlayFabWithId(tGoogleID);
            }
        }
        else
        {
            // ====================================================
            // 2. ���� �α��� ����
            // ====================================================
            Debug.LogWarning($"GPGS �α��� ����: {tStatus}");

            if (tHasSavedCustomID)
            {
                // 2-1) ���ÿ� CustomID�� ������ �� �״�� ���
                Debug.Log($"���ÿ� ����� CustomID ��� �� {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 2-2) ���ÿ� CustomID�� ������ �� ���� ID ����
                int tRandom = Random.Range(1000, 10000); // 1000~9999
                string tRandomID = $"Guest_{tRandom}";
                Debug.Log($"���ÿ� ����� ID ���� �� ���� CustomID ����: {tRandomID}");

                // JSON�� ����
                _DataSaveLoad._Data.DisplayId = tRandomID;
                _DataSaveLoad.SaveData();

                // PlayFab �α���
                LoginToPlayFabWithId(tRandomID);
            }
        }
    }


    private void LoginToPlayFabWithId(string tCustomId)
    {
        // 1) PlayFab�� CustomID�� �α���
        _PfManager.LoginWithCustomId(tCustomId);

        // 2) PlayFabLeaderboardManager ���ο��� �α��� ���� �Ŀ� DisplayName ����ȭ
        _PfManager.DisplayId = tCustomId;

        // UI ����
        var tUIManager = FindAnyObjectByType<MainSceneUIManager>();
        if (tUIManager != null)
        {
            tUIManager.UpdateUI();
        }
            
    }
}
