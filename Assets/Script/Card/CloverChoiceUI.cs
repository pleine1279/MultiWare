using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CloverChoiceUI : MonoBehaviour
{
    public static CloverChoiceUI Instance;

    [Header("UI 연결")]
    public GameObject choicePanel;        // 선택 팝업 패널
    public TMP_Text descriptionText;      // 설명 텍스트
    public Button debuffButton;           // 디버프 선택 버튼
    public Button buffButton;             // 버프 선택 버튼
    public TMP_Text debuffText;           // 디버프 버튼 텍스트
    public TMP_Text buffText;             // 버프 버튼 텍스트

    private Action onDebuffSelected;      // 디버프 선택 시 콜백
    private Action onBuffSelected;        // 버프 선택 시 콜백

    private void Awake()
    {
        Instance = this;
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    // 선택창 표시
    public void ShowChoice(
    string debuffName, string debuffDesc,
    string buffName, string buffDesc,
    Action onDebuff, Action onBuff)
    {
        onDebuffSelected = onDebuff;
        onBuffSelected = onBuff;

        if (descriptionText != null)
            descriptionText.text = "Choose Clover Effect!";

        if (debuffText != null)
            debuffText.text = $"[Debuff] {debuffName}\n{debuffDesc}";

        if (buffText != null)
            buffText.text = $"[Buff] {buffName}\n{buffDesc}";

        if (choicePanel != null)
            choicePanel.SetActive(true);
    }

    // 디버프 선택
    public void OnClickDebuff()
    {
        onDebuffSelected?.Invoke();
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    // 버프 선택
    public void OnClickBuff()
    {
        onBuffSelected?.Invoke();
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }
}