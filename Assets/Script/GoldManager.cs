using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [Header("°ñµå ¼³Á¤")]
    public int currentGold = 0;

    [Header("UI ¿¬°á")]
    public TMP_Text goldText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGoldUI();
    }

    // °ñµå È¹µæ
    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
        Debug.Log($"°ñµå È¹µæ! +{amount} / ÇöÀç °ñµå: {currentGold}");
    }

    // °ñµå »ç¿ë
    public bool SpendGold(int amount)
    {
        if (currentGold < amount)
        {
            Debug.Log("°ñµå°¡ ºÎÁ·ÇÕ´Ï´Ù!");
            return false;
        }
        currentGold -= amount;
        UpdateGoldUI();
        Debug.Log($"°ñµå »ç¿ë! -{amount} / ÇöÀç °ñµå: {currentGold}");
        return true;
    }

    // UI ¾÷µ¥ÀÌÆ®
    private void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"gold: {currentGold}";
    }
}