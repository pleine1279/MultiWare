using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI goldText;

    private void Start()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player.Instance가 null입니다! Player가 씬에 있는지 확인하세요.");
            return;
        }

        Player.Instance.OnGoldChanged += UpdateUI;
        UpdateUI(Player.Instance.gold);
    }

    private void OnDestroy()
    {
        // 구독 해제 (Player가 먼저 파괴될 수 있으니 null 체크)
        if (Player.Instance != null)
            Player.Instance.OnGoldChanged -= UpdateUI;
    }

    void UpdateUI(int gold)
    {
        goldText.text = "Gold: " + gold.ToString();
    }
}