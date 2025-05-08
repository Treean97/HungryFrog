using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class GameSceneScoreManager : MonoBehaviour
{
    public static GameSceneScoreManager _Inst;    

    private void Awake()
    {
        if(_Inst == null)
        {
            _Inst = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private int _Score;
    public int GetScore => _Score;

    [SerializeField]
    int _ShootScoreMultiple;

    [SerializeField]
    int _CombineScoreMultiple;

    [SerializeField]
    TMP_Text[] _GameSceneScoreTextUI;

    // Start is called before the first frame update
    void Start()
    {
        _Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScoreByShoot(int tObjectID)
    {
        // 0번도 있으니 +1
        _Score += (tObjectID + 1) * _ShootScoreMultiple;

        UpdateScoreUI();
    }

    public void AddScoreByCombine(int tObjectID)
    {
        // 0번도 있으니 +1
        _Score += (tObjectID + 1) * _CombineScoreMultiple;

        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        foreach (var item in _GameSceneScoreTextUI)
        {
            item.text = _Score.ToString();
        }
    }

}
