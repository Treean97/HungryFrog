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
        // 시작 시 데이터 불러오기
        LoadData();
        LoadSoundVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 데이터 저장
    public void SaveData()
    {        
        string saveData = JsonUtility.ToJson(_Data, false);
        // 저장할 경로
        string path = Application.persistentDataPath + "/PlayerData.json";

        File.WriteAllText(path, saveData);
    }

    // 데이터 불러오기
    public void LoadData()
    {
        // 불러오려는 위치 경로  
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            // 파일에 저장되어있던 문자열 가져오기
            string loadData = File.ReadAllText(path);

            // 가져온 문자열을 제이슨을 통해 클래스로 변환
            _Data = JsonUtility.FromJson<PlayerData>(loadData);
        }

        // 최초 실행 시 새로운 객체 생성
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
