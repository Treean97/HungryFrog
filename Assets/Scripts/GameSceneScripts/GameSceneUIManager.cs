using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _MenuUIGO;

    [SerializeField]
    GameObject _GameInfoUIGO;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuUI()
    {
        _MenuUIGO.SetActive(true);
    }

    public void OffMenuUI()
    {
        _MenuUIGO.SetActive(false);
    }

    public void OnGameInfoUI()
    {
        _GameInfoUIGO.SetActive(true);
    }

    public void OffGameInfoUI()
    {
        _GameInfoUIGO.SetActive(false);
    }
}
