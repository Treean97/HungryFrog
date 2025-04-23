using UnityEngine;

public class MainSceneUIGameTitle : MainSceneUIBase
{
    MainSceneManager _MainSceneManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        _MainSceneManager = GameObject.FindGameObjectWithTag("MainSceneManager").GetComponent<MainSceneManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    
    

    private void OnDestroy()
    {
        _MainSceneManager.StartRespawnGameTitle();
    }

}
