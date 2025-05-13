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
            Debug.LogWarning("�Է°��� �����ϴ�.");
            return;
        }

        Debug.Log($"�Է¹��� �ؽ�Ʈ: {tInputText}");

        // ������ ����
        PlayFabLeaderboardManager._Inst.DisplayId = tInputText;

        // UI ����
        var tUI = FindFirstObjectByType<MainSceneUIManager>();
        if (tUI != null)
        {
            tUI.UpdateUI();
        }
    }
}
