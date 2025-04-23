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

    bool _IsRespawning = false;

    [SerializeField]
    float WaitForLoadScene = 2f;

    bool _SceneChanging = false;


    private void Start()
    {
        StartRespawnGameTitle();
        ResetAllUI();


        _SceneChanging = false;
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
        _SceneChanging = true;

        yield return new WaitForSeconds(WaitForLoadScene);

        // �ε� �� �� ���� �� ��ȯ
        SceneManager.LoadScene("LoadingScene");

    }

    public void StartRespawnGameTitle()
    {
        if(!_SceneChanging)
        {
            StartCoroutine(RespawnGameTitle());
        }
        
    }

    private IEnumerator RespawnGameTitle()
    {
        _IsRespawning = true; // ����� ����

        // �������� ��ٷ� ���� ���� ���� ������Ʈ�� �Ҵ�� �ð��� Ȯ��
        yield return null;

        // ������Ʈ ����
        _GameTitleGO = Instantiate(_GameTitlePF, _GameTitleSpawnTF.position, _GameTitlePF.transform.rotation);

        _IsRespawning = false; // ����� �Ϸ�
    }

    private void ResetAllUI()
    {
        for (int i = 0; i < _MainSceneUIPF.Length; i++)
        {
            Instantiate(_MainSceneUIPF[i], _MainSceneUITF[i].position, _MainSceneUITF[i].transform.rotation);
        }
    }
}
