using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootPowerSlider : MonoBehaviour
{
    [SerializeField]
    Slider _PowerSlider;

    [SerializeField]
    ObjectShooter _PlanetShooter;

    // Start is called before the first frame update
    void Start()
    {
        _PowerSlider.maxValue = _PlanetShooter._MaxShootForce;
    }

    // Update is called once per frame
    void Update()
    {
        SetPowerSlider();
    }

    public void SetPowerSlider()
    {
        _PowerSlider.value = _PlanetShooter._ShootForce;
    }
}
