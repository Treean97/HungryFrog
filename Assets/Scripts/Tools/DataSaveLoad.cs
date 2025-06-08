using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class DataSaveLoad : MonoBehaviour
{
    // 1) 플레이어 데이터 구조체 (JSON 직렬화용)
    [System.Serializable]
    public class PlayerData
    {
        // 마스터 볼륨 값 (0~1)
        public float MasterValue = 1f;   

        // BGM 볼륨 값 (0~1)
        public float BGMValue = 1f;      

        // SFX 볼륨 값 (0~1)
        public float SFXValue = 1f;      

        // 플레이어 표시 ID
        public string DisplayId;         
    }

    public PlayerData _Data;             // 현재 로드된 플레이어 데이터
    [SerializeField] private AudioMixer _AudioMixer;  
    private string _SavePath;            // JSON 파일 저장 경로

    private void Awake()
    {
        // 2) 저장 경로 초기화
        _SavePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }

    private void Start()
    {
        // 3) 데이터 로드 및 오디오 볼륨 설정
        LoadData();
        LoadSoundVolume();
    }

    // 4) 데이터를 JSON으로 저장
    public void SaveData()
    {
        string tJson = JsonUtility.ToJson(_Data, false);
        File.WriteAllText(_SavePath, tJson);
    }

    // 5) JSON 파일에서 데이터 로드
    public void LoadData()
    {
        if (File.Exists(_SavePath))
        {
            string tJson = File.ReadAllText(_SavePath);
            _Data = JsonUtility.FromJson<PlayerData>(tJson);
        }

        // 파일이 없거나 파싱 실패 시 기본값으로 초기화
        if (_Data == null)
            _Data = new PlayerData();

        // DisplayId가 비어 있으면 기기 고유 ID로 초기화
        if (string.IsNullOrEmpty(_Data.DisplayId))
            _Data.DisplayId = SystemInfo.deviceUniqueIdentifier;
    }

    // 6) AudioMixer에 볼륨 값 적용
    public void LoadSoundVolume()
    {
        _AudioMixer.SetFloat("Master", Mathf.Log10(_Data.MasterValue) * 20f);
        _AudioMixer.SetFloat("BGM",    Mathf.Log10(_Data.BGMValue)    * 20f);
        _AudioMixer.SetFloat("SFX",    Mathf.Log10(_Data.SFXValue)    * 20f);
    }
}
