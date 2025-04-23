using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public Slider _LoadingBar;  // ���� ���¸� ǥ���� �����̴�
    public TMP_Text _LoadingText;   // ���� ���¸� ǥ���� �ؽ�Ʈ

    private void Start()
    {
        // ���� ���� �񵿱������� �ε�
        StartCoroutine(LoadGameSceneAsync("GameScene"));
    }

    IEnumerator LoadGameSceneAsync(string tSceneName)
    {
        // �񵿱� �� �ε� ����
        AsyncOperation tOperation = SceneManager.LoadSceneAsync(tSceneName);

        // �� �ε尡 �Ϸ�� ������ �ݺ�
        while (!tOperation.isDone)
        {
            // �ε�� ���� 0���� 1 ���̷� ��ȯ (50%������ �ε� �غ�, 100%�� �Ϸ�)
            float tProgress = Mathf.Clamp01(tOperation.progress / 0.9f);

            // �ٿ� �ؽ�Ʈ ������Ʈ
            _LoadingBar.value = tProgress;
            _LoadingText.text = (tProgress * 100f).ToString("F0") + "%...";

            yield return null;  // ���� �����ӱ��� ���
        }
    }
}
