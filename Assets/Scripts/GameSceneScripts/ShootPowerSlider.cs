using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootPowerSlider : MonoBehaviour
{
    [SerializeField] private Slider _PowerSlider;              // 발사력 표시 슬라이더
    [SerializeField] private ObjectShooter _PlanetShooter;     // 발사 로직 참조

    [Header("Charging Sound Settings")]
    [SerializeField] private string _ChargingSoundName = "SliderCharging"; // 충전 사운드 이름
    [SerializeField] private float _MinPitch = 1f;             // 최소 pitch
    [SerializeField] private float _MaxPitch = 2f;             // 최대 pitch

    private AudioSource _ChargingSource;                       // 반복 재생 AudioSource
    private bool _WasCharging = false;                         // 이전 프레임 충전 상태

    void Start()
    {
        _PowerSlider.maxValue = _PlanetShooter._MaxShootForce; // 슬라이더 최대값 설정
    }

    void Update()
    {
        SetPowerSlider();       // 슬라이더 값 갱신
        HandleChargingSound();  // 충전 사운드 처리
    }

    public void SetPowerSlider()
    {
        _PowerSlider.value = _PlanetShooter._ShootForce; // 현재 누적 발사력 표시
    }

    private void HandleChargingSound()
    {
        bool tIsCharging = _PlanetShooter._IsCharging; // 현재 충전 중인지

        if (tIsCharging && !_WasCharging)
        {
            // 충전 시작 시 반복 사운드 재생 및 초기 pitch 설정
            _ChargingSource = SoundManager._Inst.PlayLoopSFX(SoundCategory.Charging, _ChargingSoundName);
            if (_ChargingSource != null)
                _ChargingSource.pitch = _MinPitch;
        }

        if (tIsCharging && _ChargingSource != null)
        {
            // 충전 비율에 따라 pitch 보간
            float tRatio = _PlanetShooter._ShootForce / _PlanetShooter._MaxShootForce;
            _ChargingSource.pitch = Mathf.Lerp(_MinPitch, _MaxPitch, tRatio);
        }

        if (!tIsCharging && _WasCharging)
        {
            // 충전 종료 시 반복 사운드 정지
            if (_ChargingSource != null)
            {
                SoundManager._Inst.StopLoopSFX(_ChargingSource);
                _ChargingSource = null;
            }
        }

        _WasCharging = tIsCharging; // 상태 업데이트
    }
}
