using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneBtnManager : MonoBehaviour
{
    [SerializeField]
    float WaitForLoadScene = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // IsKinematic �ɼ� ������ OnCollisionEnter�� �ȵ�
    private void OnTriggerEnter(Collider other)
    {        

    }

    public void LoadLoadingScene()
    {
        StartCoroutine(LoadLoadingSceneCo());
    }

    IEnumerator LoadLoadingSceneCo()
    {
        yield return new WaitForSeconds(WaitForLoadScene);

        // �ε� �� �� ���� �� ��ȯ
        SceneManager.LoadScene("LoadingScene");
                
    }
}
