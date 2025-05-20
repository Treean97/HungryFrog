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
