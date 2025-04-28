using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Inst;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _BGMSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _AudioMixer;
    [SerializeField] private AudioMixerGroup _BGMMixerGroup;
    [SerializeField] private AudioMixerGroup _SFXMixerGroup;

    [Header("Sound Data List")]
    [SerializeField] private List<SoundData> _SoundDataList;

    [Header("Sound Source Pool")]
    [SerializeField] private GameObject _AudioSourcePrefab; // AudioSource ������
    [SerializeField] private int _InitialPoolSize = 10; // �ʱ� Ǯ ũ��

    private Dictionary<string, SoundData> _SoundDictionary;
    private List<AudioSource> _SoundSourcePool = new List<AudioSource>();

    private void Awake()
    {
        if (_Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);

            _SoundDictionary = new Dictionary<string, SoundData>();
            foreach (var tSoundData in _SoundDataList)
            {
                if (!_SoundDictionary.ContainsKey(tSoundData._SoundName))
                {
                    _SoundDictionary.Add(tSoundData._SoundName, tSoundData);
                }
            }

            // �ʱ� Ǯ ����
            for (int tI = 0; tI < _InitialPoolSize; tI++)
            {
                CreateNewSoundSource();
            }

            // BGM �ҽ��� �ͼ� �׷� ����
            _BGMSource.outputAudioMixerGroup = _BGMMixerGroup;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private AudioSource CreateNewSoundSource()
    {
        GameObject tSourceObj = Instantiate(_AudioSourcePrefab, transform);
        AudioSource tSource = tSourceObj.GetComponent<AudioSource>();
        tSource.outputAudioMixerGroup = _SFXMixerGroup; // SFX �ҽ��� Effect �׷� ����
        _SoundSourcePool.Add(tSource);
        return tSource;
    }

    private AudioSource GetAvailableSoundSource()
    {
        foreach (var tSource in _SoundSourcePool)
        {
            if (!tSource.isPlaying)
            {
                return tSource;
            }
        }

        // ��� ������ AudioSource�� ������ ���� �����
        return CreateNewSoundSource();
    }

    public void PlaySFX(string tSoundName)
    {
        if (_SoundDictionary.TryGetValue(tSoundName, out var tSoundData))
        {
            AudioSource tSfxSource = GetAvailableSoundSource();
            tSfxSource.PlayOneShot(tSoundData._Clip, tSoundData._Volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{tSoundName}'�� ã�� �� �����ϴ�.");
        }
    }

    public void PlayBGM(string tSoundName)
    {
        if (_SoundDictionary.TryGetValue(tSoundName, out var tSoundData))
        {
            if (_BGMSource.clip == tSoundData._Clip && _BGMSource.isPlaying)
                return;

            _BGMSource.clip = tSoundData._Clip;
            _BGMSource.volume = tSoundData._Volume;
            _BGMSource.loop = true;
            _BGMSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{tSoundName}'�� ã�� �� �����ϴ�.");
        }
    }

    public void StopBGM()
    {
        _BGMSource.Stop();
    }

    public void SetBGMVolume(float tVolume)
    {
        _AudioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(tVolume, 0.0001f, 1f)) * 20f);
    }

    public void SetSFXVolume(float tVolume)
    {
        _AudioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(tVolume, 0.0001f, 1f)) * 20f);
    }
}
