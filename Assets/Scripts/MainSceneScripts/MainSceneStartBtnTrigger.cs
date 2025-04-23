using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneStartBtnTrigger : MonoBehaviour
{   
    MainSceneManager _MainSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        _MainSceneManager = GameObject.FindGameObjectWithTag("MainSceneManager").GetComponent<MainSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        _MainSceneManager.LoadLoadingScene();
    }
}
