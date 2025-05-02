using Unity.VisualScripting;
using UnityEngine;

public class MainSceneSettingBtnTrigger : MonoBehaviour
{
    MainSceneUIManager _MainSceneUIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MainSceneUIManager = GameObject.FindGameObjectWithTag("MainSceneUIManager").GetComponent<MainSceneUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDestroy()
    {
        _MainSceneUIManager.SettingUIOn();
    }

}
