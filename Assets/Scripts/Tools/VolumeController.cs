using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("슬라이더")]
    [SerializeField] private Slider _MasterSlider;
    [SerializeField] private Slider _BGMSlider;
    [SerializeField] private Slider _SFXSlider;

    [Header("뮤트 버튼")]
    [SerializeField] private Button _MasterMuteButton;
    [SerializeField] private Button _BGMMuteButton;
    [SerializeField] private Button _SFXMuteButton;

    [Header("공용 스프라이트")]
    [SerializeField] private Sprite _MuteSprite;
    [SerializeField] private Sprite _UnmuteSprite;

    private bool _IsMasterMuted = false;
    private bool _IsBGMMuted = false;
    private bool _IsSFXMuted = false;

    private void Start()
    {
        float tMasterDb, tBGMDb, tSFXDb;
        SoundManager._Inst._AudioMixer.GetFloat("Master", out tMasterDb);
        SoundManager._Inst._AudioMixer.GetFloat("BGM", out tBGMDb);
        SoundManager._Inst._AudioMixer.GetFloat("SFX", out tSFXDb);

        _MasterSlider.value = DbTo01(tMasterDb);
        _BGMSlider.value = DbTo01(tBGMDb);
        _SFXSlider.value = DbTo01(tSFXDb);

        _MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        _BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        _SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        _MasterMuteButton.onClick.AddListener(ToggleMasterMute);
        _BGMMuteButton.onClick.AddListener(ToggleBGMMute);
        _SFXMuteButton.onClick.AddListener(ToggleSFXMute);

        UpdateButtonIcon(_MasterMuteButton, _IsMasterMuted);
        UpdateButtonIcon(_BGMMuteButton, _IsBGMMuted);
        UpdateButtonIcon(_SFXMuteButton, _IsSFXMuted);
    }

    private void SetMasterVolume(float tValue)
    {
        if (_IsMasterMuted)
            return;

        SoundManager._Inst.SetMasterVolume(tValue);
        UpdateButtonIcon(_MasterMuteButton, false);
    }

    private void SetBGMVolume(float tValue)
    {
        if (_IsBGMMuted)
            return;

        SoundManager._Inst.SetBGMVolume(tValue);
        UpdateButtonIcon(_BGMMuteButton, false);
    }

    private void SetSFXVolume(float tValue)
    {
        if (_IsSFXMuted)
            return;

        SoundManager._Inst.SetSFXVolume(tValue);
        UpdateButtonIcon(_SFXMuteButton, false);
    }

    private void ToggleMasterMute()
    {
        _IsMasterMuted = !_IsMasterMuted;

        if (_IsMasterMuted)
        {
            SoundManager._Inst._AudioMixer.SetFloat("Master", -80f);
        }
        else
        {
            SetMasterVolume(_MasterSlider.value);
        }

        UpdateButtonIcon(_MasterMuteButton, _IsMasterMuted);
    }

    private void ToggleBGMMute()
    {
        _IsBGMMuted = !_IsBGMMuted;

        if (_IsBGMMuted)
        {
            SoundManager._Inst._AudioMixer.SetFloat("BGM", -80f);
        }
        else
        {
            SetBGMVolume(_BGMSlider.value);
        }

        UpdateButtonIcon(_BGMMuteButton, _IsBGMMuted);
    }

    private void ToggleSFXMute()
    {
        _IsSFXMuted = !_IsSFXMuted;

        if (_IsSFXMuted)
        {
            SoundManager._Inst._AudioMixer.SetFloat("SFX", -80f);
        }
        else
        {
            SetSFXVolume(_SFXSlider.value);
        }

        UpdateButtonIcon(_SFXMuteButton, _IsSFXMuted);
    }

    private void UpdateButtonIcon(Button tButton, bool tIsMuted)
    {
        if (tButton.TryGetComponent<Image>(out var tImage))
        {
            tImage.sprite = tIsMuted ? _MuteSprite : _UnmuteSprite;
        }
    }

    private float DbTo01(float tDb)
    {
        return Mathf.Pow(10f, tDb / 20f);
    }
}
