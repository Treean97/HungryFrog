using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Inst;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _BGMSource;

    [Header("Audio Mixer")]
    [SerializeField] public AudioMixer _AudioMixer;
    [SerializeField] private AudioMixerGroup _BGMMixerGroup;
    [SerializeField] private AudioMixerGroup _SFXMixerGroup;

    [Header("Sound Data List")]
    [SerializeField] private List<SoundData> _SoundDataList;

    [Header("Sound Source Pool")]
    [SerializeField] private GameObject _AudioSourcePrefab;
    [SerializeField] private int _InitialPoolSize = 10;



    private Dictionary<SoundCategory, Dictionary<string, SoundData>> _SoundCategoryDictionary;
    private List<AudioSource> _SoundSourcePool = new List<AudioSource>();

    private void Awake()
    {
        if (_Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);

            _SoundCategoryDictionary = new Dictionary<SoundCategory, Dictionary<string, SoundData>>();

            foreach (var tSoundData in _SoundDataList)
            {
                // 카테고리 별로 딕셔너리에 정리
                if (!_SoundCategoryDictionary.ContainsKey(tSoundData._Category))
                    _SoundCategoryDictionary.Add(tSoundData._Category, new Dictionary<string, SoundData>());

                if (!_SoundCategoryDictionary[tSoundData._Category].ContainsKey(tSoundData._SoundName))
                    _SoundCategoryDictionary[tSoundData._Category].Add(tSoundData._SoundName, tSoundData);
            }

            for (int tI = 0; tI < _InitialPoolSize; tI++)
                CreateNewSoundSource();

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
        tSource.outputAudioMixerGroup = _SFXMixerGroup;
        _SoundSourcePool.Add(tSource);
        return tSource;
    }

    private AudioSource GetAvailableSoundSource()
    {
        foreach (var tSource in _SoundSourcePool)
        {
            if (!tSource.isPlaying)
                return tSource;
        }

        return CreateNewSoundSource();
    }

    public void PlaySFX(SoundCategory tCategory, string tSoundName)
    {
        if (_SoundCategoryDictionary.TryGetValue(tCategory, out var tCategoryDict))
        {
            if (tCategoryDict.TryGetValue(tSoundName, out var tSoundData))
            {
                AudioSource tSfxSource = GetAvailableSoundSource();
                tSfxSource.PlayOneShot(tSoundData._Clip, tSoundData._Volume);
            }
            else
            {
                Debug.LogWarning($"SFX '{tSoundName}'를 찾을 수 없습니다. (Category: {tCategory})");
            }
        }
        else
        {
            Debug.LogWarning($"'{tCategory}' 카테고리가 없습니다.");
        }
    }

    public AudioSource PlayLoopSFX(SoundCategory tCategory, string tSoundName)
    {
        if (_SoundCategoryDictionary.TryGetValue(tCategory, out var tCategoryDict))
        {
            if (tCategoryDict.TryGetValue(tSoundName, out var tSoundData))
            {
                AudioSource tSfxSource = GetAvailableSoundSource();
                tSfxSource.clip = tSoundData._Clip;
                tSfxSource.volume = tSoundData._Volume;
                tSfxSource.loop = true;
                tSfxSource.Play();
                return tSfxSource; // 호출한 쪽에서 pitch 제어 가능
            }
            else
            {
                Debug.LogWarning($"SFX '{tSoundName}'를 찾을 수 없습니다. (Category: {tCategory})");
            }
        }
        else
        {
            Debug.LogWarning($"'{tCategory}' 카테고리가 없습니다.");
        }

        return null;
    }

    public void StopLoopSFX(AudioSource tSource)
    {
        if (tSource != null)
        {
            tSource.Stop();
            tSource.clip = null;
            tSource.loop = false;
            tSource.pitch = 1f;
        }
    }


    public void PlayRandomSFX(SoundCategory tCategory)
    {
        if (_SoundCategoryDictionary.TryGetValue(tCategory, out var tCategoryDict))
        {
            var soundList = new List<SoundData>(tCategoryDict.Values);

            if (soundList.Count == 0)
            {
                Debug.LogWarning($"'{tCategory}' 카테고리에 사운드가 없습니다.");
                return;
            }

            var randomSound = soundList[Random.Range(0, soundList.Count)];
            AudioSource tSfxSource = GetAvailableSoundSource();
            tSfxSource.PlayOneShot(randomSound._Clip, randomSound._Volume);
        }
        else
        {
            Debug.LogWarning($"'{tCategory}' 카테고리가 없습니다.");
        }
    }

    public void PlayBGM(string tSoundName)
    {
        if (_SoundCategoryDictionary.TryGetValue(SoundCategory.BGM, out var tCategoryDict))
        {
            if (tCategoryDict.TryGetValue(tSoundName, out var tSoundData))
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
                Debug.LogWarning($"BGM '{tSoundName}'를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"BGM 카테고리가 없습니다.");
        }
    }

    public void StopBGM()
    {
        _BGMSource.Stop();
    }

    public void SetMasterVolume(float value01)
    {
        _AudioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(value01, 0.0001f, 1f)) * 20f);
    }

    public void SetBGMVolume(float value01)
    {
        _AudioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(value01, 0.0001f, 1f)) * 20f);
    }

    public void SetSFXVolume(float value01)
    {
        _AudioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(value01, 0.0001f, 1f)) * 20f);
    }


}
