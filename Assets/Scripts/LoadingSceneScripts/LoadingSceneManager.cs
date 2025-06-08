using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    // 로딩바 슬라이더 참조
    public Slider _LoadingBar;

    // 로딩 진행 텍스트 참조
    public TMP_Text _LoadingText;

    void Start()
    {
        // 게임 씬 비동기 로드 시작
        StartCoroutine(LoadGameSceneAsync("GameScene"));
    }

    IEnumerator LoadGameSceneAsync(string tSceneName)
    {
        // 비동기 로드 연산 실행
        AsyncOperation tOperation = SceneManager.LoadSceneAsync(tSceneName);

        // 로드 완료 전까지 진행률 업데이트
        while (!tOperation.isDone)
        {
            // progress는 0~0.9 범위이므로 0~1로 보정
            float tProgress = Mathf.Clamp01(tOperation.progress / 0.9f);

            // UI에 진행률 표시
            _LoadingBar.value = tProgress;
            _LoadingText.text  = (tProgress * 100f).ToString("F0") + "%...";

            yield return null; // 다음 프레임까지 대기
        }
    }
}
