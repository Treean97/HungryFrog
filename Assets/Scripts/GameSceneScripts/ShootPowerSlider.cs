using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootPowerSlider : MonoBehaviour
{
    [SerializeField] private Slider _PowerSlider;
    [SerializeField] private ObjectShooter _PlanetShooter;

    [Header("Charging Sound Settings")]
    [SerializeField] private string _ChargingSoundName = "SliderCharging";
    [SerializeField] private float _MinPitch = 1f;
    [SerializeField] private float _MaxPitch = 2f;

    private AudioSource _ChargingSource;
    private bool _WasCharging = false;

    void Start()
    {
        _PowerSlider.maxValue = _PlanetShooter._MaxShootForce;
    }

    void Update()
    {
        SetPowerSlider();
        HandleChargingSound();
    }

    public void SetPowerSlider()
    {
        _PowerSlider.value = _PlanetShooter._ShootForce;
    }

    private void HandleChargingSound()
    {
        bool tIsCharging = _PlanetShooter._IsCharging;

        if (tIsCharging && !_WasCharging)
        {
            // 충전 시작
            _ChargingSource = SoundManager._Inst.PlayLoopSFX(SoundCategory.Charging, _ChargingSoundName);
            if (_ChargingSource != null)
                _ChargingSource.pitch = _MinPitch;
        }

        if (tIsCharging && _ChargingSource != null)
        {
            // pitch는 힘 비율에 따라 조절
            float tRatio = _PlanetShooter._ShootForce / _PlanetShooter._MaxShootForce;
            _ChargingSource.pitch = Mathf.Lerp(_MinPitch, _MaxPitch, tRatio);
        }

        if (!tIsCharging && _WasCharging)
        {
            // 충전 종료
            if (_ChargingSource != null)
            {
                SoundManager._Inst.StopLoopSFX(_ChargingSource);
                _ChargingSource = null;
            }
        }

        _WasCharging = tIsCharging;
    }
}
