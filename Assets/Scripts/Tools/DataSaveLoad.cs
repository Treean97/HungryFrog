using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class DataSaveLoad : MonoBehaviour
{
    public class PlayerData
    {
        public float MasterValue = 1;
        public float BGMValue = 1;
        public float SFXValue = 1;
    }

    public PlayerData _Data;

    [SerializeField]
    AudioMixer _AudioMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� �� ������ �ҷ�����
        LoadData();
        LoadSoundVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ������ ����
    public void SaveData()
    {        
        string saveData = JsonUtility.ToJson(_Data, false);
        // ������ ���
        string path = Application.persistentDataPath + "/PlayerData.json";

        File.WriteAllText(path, saveData);
    }

    // ������ �ҷ�����
    public void LoadData()
    {
        // �ҷ������� ��ġ ���  
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            // ���Ͽ� ����Ǿ��ִ� ���ڿ� ��������
            string loadData = File.ReadAllText(path);

            // ������ ���ڿ��� ���̽��� ���� Ŭ������ ��ȯ
            _Data = JsonUtility.FromJson<PlayerData>(loadData);
        }

        // ���� ���� �� ���ο� ��ü ����
        if (_Data == null)
        {
            _Data = new PlayerData();
        }

    }

    public void LoadSoundVolume()
    {
        _AudioMixer.SetFloat("Master", Mathf.Log10(_Data.MasterValue) * 20);
        _AudioMixer.SetFloat("BGM", Mathf.Log10(_Data.BGMValue) * 20);
        _AudioMixer.SetFloat("SFX", Mathf.Log10(_Data.SFXValue) * 20);
    }
}
