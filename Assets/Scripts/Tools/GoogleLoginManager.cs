using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleLoginManager : MonoBehaviour
{
    // ������ ����/�ε� ���� Ŭ���� ����
    private DataSaveLoad _DataSaveLoad;

    // PlayFab �������� �Ŵ��� �̱��� ����
    private PlayFabLeaderboardManager _PfManager;

    // ��� ���� ID �����
    private string _DeviceUID;

    void Start()
    {
        // 1) ������Ʈ/�Ŵ��� ���� ã��
        _DataSaveLoad = FindAnyObjectByType<DataSaveLoad>();
        _PfManager     = PlayFabLeaderboardManager._Inst;

        if (_DataSaveLoad == null)
            Debug.LogError("DataSaveLoad ������Ʈ�� ã�� ���߽��ϴ�!");
        if (_PfManager == null)
            Debug.LogError("PlayFabLeaderboardManager �ν��Ͻ��� ã�� ���߽��ϴ�!");

        // 2) ��� ���� ID ��������
        _DeviceUID = SystemInfo.deviceUniqueIdentifier;

        // 3) GPGS (Google Play Games Services) �ʱ�ȭ
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        // 4) GPGS �α��� �õ� �Ǵ� �̹� �α��� ���� ó��
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        else
        {
            // �̹� �α��ε� ���¸� �ٷ� ó��
            ProcessAuthentication(SignInStatus.Success);
        }
    }

    /// GPGS �α��� ��� �ݹ�
    void ProcessAuthentication(SignInStatus tStatus)
    {
        // 1) ����� CustomID �ҷ�����
        _DataSaveLoad.LoadData();
        string tSavedID = _DataSaveLoad._Data.DisplayId;

        bool tHasSavedCustomID =
            !string.IsNullOrEmpty(tSavedID) && (tSavedID != _DeviceUID);

        if (tStatus == SignInStatus.Success)
        {
            Debug.Log("GPGS �α��� ����!");

            if (tHasSavedCustomID)
            {
                // 1-1) ������ ����� CustomID�� PlayFab �α���
                Debug.Log($"����� CustomID ���: {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 1-2) ���� ID�� �� CustomID ���� �� ����, PlayFab �α���
                string tGoogleID = PlayGamesPlatform.Instance.GetUserId();
                Debug.Log($"Google ID ���: {tGoogleID}");

                _DataSaveLoad._Data.DisplayId = tGoogleID;
                _DataSaveLoad.SaveData();

                LoginToPlayFabWithId(tGoogleID);
            }
        }
        else
        {
            Debug.LogWarning($"GPGS �α��� ����: {tStatus}");

            if (tHasSavedCustomID)
            {
                // 2-1) �α��� ���� �ÿ��� ����� CustomID�� PlayFab �α���
                Debug.Log($"����� CustomID ����: {tSavedID}");
                LoginToPlayFabWithId(tSavedID);
            }
            else
            {
                // 2-2) ��ȸ�� ���� Guest ID ����, ����, PlayFab �α���
                int    tRandom   = Random.Range(1000, 10000);
                string tRandomID = $"Guest_{tRandom}";
                Debug.Log($"���� Guest ID ����: {tRandomID}");

                _DataSaveLoad._Data.DisplayId = tRandomID;
                _DataSaveLoad.SaveData();

                LoginToPlayFabWithId(tRandomID);
            }
        }
    }

    /// PlayFab�� CustomID�� �α����ϰ�, UI�� ID ǥ�ñ��� ó��
    private void LoginToPlayFabWithId(string tCustomId)
    {
        // 1) PlayFab CustomID �α��� ȣ��
        _PfManager.LoginWithCustomId(tCustomId);

        // 2) PlayFabLeaderboardManager �� DisplayId ����
        _PfManager.DisplayId = tCustomId;

        // 3) ���� UI�� �α��ε� ID ������Ʈ
        var tUIManager = FindAnyObjectByType<MainSceneUIManager>();
        if (tUIManager != null)
        {
            tUIManager.UpdateUI();
        }
    }
}
