using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject _GameTitlePF;

    GameObject _GameTitleGO;

    [SerializeField]
    Transform _GameTitleSpawnTF;

    [SerializeField]
    GameObject[] _MainSceneUIPF;

    [SerializeField]
    Transform[] _MainSceneUITF;


    [SerializeField]
    float WaitForLoadScene = 2f;

    bool _IsSceneChanging = false;
    public bool GetIsSceneChanging => _IsSceneChanging;


    private void Start()
    {
        RespawnGameTitle();
        ResetAllUI();


        _IsSceneChanging = false;

        // 사운드
        SoundManager._Inst.PlayBGM("MainSceneBGM");
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void LoadLoadingScene()
    {
        StartCoroutine(LoadLoadingSceneCo());
    }


    IEnumerator LoadLoadingSceneCo()
    {
        _IsSceneChanging = true;

        yield return new WaitForSeconds(WaitForLoadScene);

        // 로딩 씬 후 게임 씬 전환
        SceneManager.LoadScene("LoadingScene");

    }

    public void RespawnGameTitle()
    {
        if(!_IsSceneChanging)
        {
            // 오브젝트 생성
            _GameTitleGO = Instantiate(_GameTitlePF, _GameTitleSpawnTF.position, _GameTitlePF.transform.rotation);
        }
        
    }



    public void RespawnUIObject(int tObjectIndex, float tDelay)
    {
        if (!_IsSceneChanging)
        {
            StartCoroutine(RespawnUIObjectCo(tObjectIndex, tDelay));

        }

    }

    IEnumerator RespawnUIObjectCo(int tObjectIndex, float tDelay)
    {
        yield return new WaitForSeconds(tDelay);

        // 오브젝트 생성
        Instantiate(_MainSceneUIPF[tObjectIndex], _MainSceneUITF[tObjectIndex]);
    }


    private void ResetAllUI()
    {
        for (int i = 0; i < _MainSceneUIPF.Length; i++)
        {
            Instantiate(_MainSceneUIPF[i], _MainSceneUITF[i].position, _MainSceneUITF[i].transform.rotation);
        }
    }
}
