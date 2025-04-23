using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public Slider _LoadingBar;  // 진행 상태를 표시할 슬라이더
    public TMP_Text _LoadingText;   // 진행 상태를 표시할 텍스트

    private void Start()
    {
        // 게임 씬을 비동기적으로 로드
        StartCoroutine(LoadGameSceneAsync("GameScene"));
    }

    IEnumerator LoadGameSceneAsync(string tSceneName)
    {
        // 비동기 씬 로드 시작
        AsyncOperation tOperation = SceneManager.LoadSceneAsync(tSceneName);

        // 씬 로드가 완료될 때까지 반복
        while (!tOperation.isDone)
        {
            // 로드된 양을 0에서 1 사이로 반환 (50%까지는 로딩 준비, 100%가 완료)
            float tProgress = Mathf.Clamp01(tOperation.progress / 0.9f);

            // 바와 텍스트 업데이트
            _LoadingBar.value = tProgress;
            _LoadingText.text = (tProgress * 100f).ToString("F0") + "%...";

            yield return null;  // 다음 프레임까지 대기
        }
    }
}
