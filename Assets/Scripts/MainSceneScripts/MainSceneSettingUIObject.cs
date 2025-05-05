using System.Collections;
using UnityEngine;

public class MainSceneSettingUIObject : MainSceneUIObjectBase
{
    MainSceneManager _MainSceneManager;
    MainSceneUIManager _MainSceneUIManager;
    DissolveEffect _DissolveEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _MainSceneManager = GameObject.FindGameObjectWithTag("MainSceneManager").GetComponent<MainSceneManager>();
        _MainSceneUIManager = GameObject.FindGameObjectWithTag("MainSceneUIManager").GetComponent<MainSceneUIManager>();
        _DissolveEffect = GetComponent<DissolveEffect>();

        StartCoroutine(RespwanCo());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    IEnumerator RespwanCo()
    {
        // »ç¿îµå
        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "MainSceneObjectSpawn");

        yield return StartCoroutine(_DissolveEffect.DissolveInCo());

        gameObject.layer = LayerMask.NameToLayer("Sliceable");
    }

    private void OnDestroy()
    {
        if (_MainSceneManager == null)
        {
            return;
        }

        if (!_MainSceneManager.GetIsSceneChanging)
        {
            _MainSceneUIManager.SettingUIOn();
            _MainSceneManager.RespawnUIObject(2, _RespawnDelay);
        }        
    }

}
