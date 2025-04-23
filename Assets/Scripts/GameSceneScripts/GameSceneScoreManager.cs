using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [SerializeField]
    GameSceneScoreUI _GameSceneScoreUI;

    [SerializeField]
    int _ShootScoreMultiple;

    [SerializeField]
    int _CombineScoreMultiple;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScoreByShoot(int tObjectID)
    {
        // 0번도 있으니 +1
        _GameSceneScoreUI.AddScore((tObjectID + 1) * _ShootScoreMultiple);
    }

    public void AddScoreByCombine(int tObjectID)
    {
        // 0번도 있으니 +1
        _GameSceneScoreUI.AddScore((tObjectID + 1) * _CombineScoreMultiple);
        
    }
}
