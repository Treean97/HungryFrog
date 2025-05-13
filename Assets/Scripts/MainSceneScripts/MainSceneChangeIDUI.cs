using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneChangeIDUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _InputField;
    [SerializeField] private Button _SubmitButton;

    private void Start()
    {
        _SubmitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string tInputText = _InputField.text.Trim();

        if (string.IsNullOrEmpty(tInputText))
        {
            Debug.LogWarning("입력값이 없습니다.");
            return;
        }

        Debug.Log($"입력받은 텍스트: {tInputText}");

        // 데이터 저장
        PlayFabLeaderboardManager._Inst.DisplayId = tInputText;

        // UI 갱신
        var tUI = FindFirstObjectByType<MainSceneUIManager>();
        if (tUI != null)
        {
            tUI.UpdateUI();
        }
    }
}
