using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneBtnManager : MonoBehaviour
{
    [SerializeField]
    GameSceneUIManager _GameSceneUIManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuBtn()
    {
        _GameSceneUIManager.OnMenuUI();
    }

    public void OffMenuBtn()
    {
        _GameSceneUIManager.OffMenuUI();
    }

    public void OnGameInfoBtn()
    {
        _GameSceneUIManager.OnGameInfoUI();
    }

    public void OffGameInfoBtn()
    {
        _GameSceneUIManager.OffGameInfoUI();
    }
}
