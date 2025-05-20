using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class DataSaveLoad : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public float MasterValue = 1f;      // 마스터 볼륨
        public float BGMValue = 1f;      // BGM 볼륨
        public float SFXValue = 1f;      // SFX 볼륨
        public string DisplayId;            // 플레이어 표시 ID
    }

    public PlayerData _Data;                // 현재 플레이어 데이터 인스턴스
    [SerializeField] private AudioMixer _AudioMixer;
    private string _SavePath;               // JSON 파일 저장 경로


    private void Start()
    {
        // persistentDataPath 하위에 JSON 파일 경로를 설정
        _SavePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");

        LoadData();        // 시작 시 디스크에서 불러오기
        LoadSoundVolume(); // 볼륨 설정 적용

    }

    public void SaveData()
    {
        // PlayerData 객체를 JSON으로 직렬화하여 파일에 저장
        string tJson = JsonUtility.ToJson(_Data, false);
        File.WriteAllText(_SavePath, tJson);
    }

    public void LoadData()
    {
        // 파일이 존재하면 JSON 읽어서 역직렬화
        if (File.Exists(_SavePath))
        {
            string tJson = File.ReadAllText(_SavePath);
            _Data = JsonUtility.FromJson<PlayerData>(tJson);
        }

        // 파일이 없거나 역직렬화 실패 시 새 객체 생성
        if (_Data == null)
            _Data = new PlayerData();

        // DisplayId가 비어 있으면 디바이스 고유 ID로 초기화
        if (string.IsNullOrEmpty(_Data.DisplayId))
            _Data.DisplayId = SystemInfo.deviceUniqueIdentifier;
    }

    public void LoadSoundVolume()
    {
        // AudioMixer에 JSON에서 불러온 볼륨 설정값 적용
        _AudioMixer.SetFloat("Master", Mathf.Log10(_Data.MasterValue) * 20f);
        _AudioMixer.SetFloat("BGM", Mathf.Log10(_Data.BGMValue) * 20f);
        _AudioMixer.SetFloat("SFX", Mathf.Log10(_Data.SFXValue) * 20f);
    }
}
