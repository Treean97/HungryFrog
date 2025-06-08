using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneChangeIDUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _InputField; // 사용자 입력 필드
    [SerializeField] private Button _SubmitButton;       // 제출 버튼

    private void Start()
    {
        // 버튼 클릭 시 OnSubmit 호출
        _SubmitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        // 입력값 앞뒤 공백 제거
        string tInputText = _InputField.text.Trim();

        // 입력값 비어 있으면 경고 후 종료
        if (string.IsNullOrEmpty(tInputText))
        {
            Debug.LogWarning("입력값이 없습니다.");
            return;
        }

        // 디버그용 입력 텍스트 출력
        Debug.Log($"입력한 텍스트: {tInputText}");

        // PlayFab에 표시 ID 저장
        PlayFabLeaderboardManager._Inst.DisplayId = tInputText;

        // UI 매니저가 있으면 화면 갱신
        var tUI = FindFirstObjectByType<MainSceneUIManager>();
        if (tUI != null)
        {
            tUI.UpdateUI();
        }
    }
}
