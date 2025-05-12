//using System.IO;
//using UnityEngine;
//using UnityEngine.Audio;

//public class DataSaveLoad : MonoBehaviour
//{
//    public class PlayerData
//    {
//        public float MasterValue = 1;
//        public float BGMValue = 1;
//        public float SFXValue = 1;

//        public string DisplayId;
//    }

//    public PlayerData _Data;

//    [SerializeField]
//    AudioMixer _AudioMixer;


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        // ���� �� ������ �ҷ�����
//        LoadData();
//        LoadSoundVolume();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//// ������ ����
//public void SaveData()
//{        
//    string saveData = JsonUtility.ToJson(_Data, false);
//    // ������ ���
//    string path = Application.persistentDataPath + "/PlayerData.json";

//    File.WriteAllText(path, saveData);
//}

// ������ �ҷ�����
//public void LoadData()
//{
//    // �ҷ������� ��ġ ���  
//    string path = Application.persistentDataPath + "/PlayerData.json";

//    if (File.Exists(path))
//    {
//        // ���Ͽ� ����Ǿ��ִ� ���ڿ� ��������
//        string loadData = File.ReadAllText(path);

//        // ������ ���ڿ��� ���̽��� ���� Ŭ������ ��ȯ
//        _Data = JsonUtility.FromJson<PlayerData>(loadData);
//    }

//    // ���� ���� �� ���ο� ��ü ����
//    if (_Data == null)
//    {
//        _Data = new PlayerData();
//    }

//}

//public void LoadSoundVolume()
//{
//    _AudioMixer.SetFloat("Master", Mathf.Log10(_Data.MasterValue) * 20);
//    _AudioMixer.SetFloat("BGM", Mathf.Log10(_Data.BGMValue) * 20);
//    _AudioMixer.SetFloat("SFX", Mathf.Log10(_Data.SFXValue) * 20);
//}
//}

// DataSaveLoad.cs
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class DataSaveLoad : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public float MasterValue = 1f;      // ������ ����
        public float BGMValue = 1f;      // BGM ����
        public float SFXValue = 1f;      // SFX ����
        public string DisplayId;            // �÷��̾� ǥ�� ID
    }

    public PlayerData _Data;                // ���� �÷��̾� ������ �ν��Ͻ�
    [SerializeField] private AudioMixer _AudioMixer;
    private string _SavePath;               // JSON ���� ���� ���


    private void Start()
    {
        // persistentDataPath ������ JSON ���� ��θ� ����
        _SavePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");

        LoadData();        // ���� �� ��ũ���� �ҷ�����
        LoadSoundVolume(); // ���� ���� ����

        PlayFabLeaderboardManager._Inst.Init();

    }

    public void SaveData()
    {
        // PlayerData ��ü�� JSON���� ����ȭ�Ͽ� ���Ͽ� ����
        string tJson = JsonUtility.ToJson(_Data, false);
        File.WriteAllText(_SavePath, tJson);
    }

    public void LoadData()
    {
        // ������ �����ϸ� JSON �о ������ȭ
        if (File.Exists(_SavePath))
        {
            string tJson = File.ReadAllText(_SavePath);
            _Data = JsonUtility.FromJson<PlayerData>(tJson);
        }

        // ������ ���ų� ������ȭ ���� �� �� ��ü ����
        if (_Data == null)
            _Data = new PlayerData();

        // DisplayId�� ��� ������ ����̽� ���� ID�� �ʱ�ȭ
        if (string.IsNullOrEmpty(_Data.DisplayId))
            _Data.DisplayId = SystemInfo.deviceUniqueIdentifier;
    }

    public void LoadSoundVolume()
    {
        // AudioMixer�� JSON���� �ҷ��� ���� ������ ����
        _AudioMixer.SetFloat("Master", Mathf.Log10(_Data.MasterValue) * 20f);
        _AudioMixer.SetFloat("BGM", Mathf.Log10(_Data.BGMValue) * 20f);
        _AudioMixer.SetFloat("SFX", Mathf.Log10(_Data.SFXValue) * 20f);
    }
}
