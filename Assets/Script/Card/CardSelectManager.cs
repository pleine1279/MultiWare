using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSelectManager : MonoBehaviour
{
    public static CardSelectManager Instance;

    [Header("선택 설정")]
    public int maxSelectCount = 5;

    [Header("UI 연결")]
    public Button useCardButton;
    public TMP_Text handResultText;
    public Canvas mainCanvas;

    private List<CardView> selectedCards = new List<CardView>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void SelectCard(CardView card)
    {
        if (selectedCards.Count >= maxSelectCount)
        {
            Debug.Log("Max 5 cards!");
            return;
        }

        selectedCards.Add(card);
        Debug.Log($"Selected: {card.cardData.cardName} ({selectedCards.Count})");
        UpdateUI();
    }

    public void DeselectCard(CardView card)
    {
        selectedCards.Remove(card);
        Debug.Log($"Deselected: {card.cardData.cardName} ({selectedCards.Count})");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (selectedCards.Count == 0)
        {
            if (handResultText != null)
                handResultText.text = "Select Cards";
            if (useCardButton != null)
                useCardButton.interactable = false;
            return;
        }

        List<CardData> cardDataList = new List<CardData>();
        foreach (CardView cv in selectedCards)
            cardDataList.Add(cv.cardData);

        HandResult result = HandEvaluator.Evaluate(cardDataList);

        if (handResultText != null)
            handResultText.text = $"{result.GetRankName()} (DMG: {result.baseDamage})";

        if (useCardButton != null)
            useCardButton.interactable = true;

        Debug.Log($"Now: {result.GetRankName()} / Damage: {result.baseDamage}");
    }

    public void OnClickUseCard()
    {
        if (selectedCards.Count == 0) return;

        // 족보 판정
        List<CardData> cardDataList = new List<CardData>();
        foreach (CardView cv in selectedCards)
            cardDataList.Add(cv.cardData);

        HandResult result = HandEvaluator.Evaluate(cardDataList);
        Debug.Log($"족보 사용! {result.GetRankName()} / 데미지: {result.baseDamage}");

        // 카드 뭉탱이 생성
        CreateCardBundle(result, cardDataList);

        // 선택한 카드 제거
        List<CardView> cardsToRemove = new List<CardView>(selectedCards);
        foreach (CardView cv in cardsToRemove)
        {
            if (cv != null && cv.gameObject != null)
                Destroy(cv.gameObject);
        }
        selectedCards.Clear();

        UpdateUI();
    }

    // 카드 뭉탱이 생성
    private void CreateCardBundle(HandResult result, List<CardData> cardDataList)
    {
        GameObject bundleObj = new GameObject("CardBundle");

        // mainCanvas가 연결되어 있으면 사용, 없으면 찾기
        Canvas canvas = mainCanvas != null ?
            mainCanvas : FindFirstObjectByType<Canvas>();

        bundleObj.transform.SetParent(canvas.transform, false);
        bundleObj.transform.SetAsLastSibling();

        RectTransform rect = bundleObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(120, 180);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, -200f);

        Image img = bundleObj.AddComponent<Image>();
        img.color = new Color(1f, 0.8f, 0f, 0.9f);
        img.raycastTarget = true;

        CardBundle bundle = bundleObj.AddComponent<CardBundle>();
        bundle.Setup(result, cardDataList);

        GameObject textObj = new GameObject("BundleText");
        textObj.transform.SetParent(bundleObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        TMPro.TMP_Text text = textObj.AddComponent<TMPro.TextMeshProUGUI>();
        text.text = $"{result.GetRankName()}\nDMG:{result.baseDamage}";
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.fontSize = 14;
        text.raycastTarget = false;
        text.color = Color.black;

        Debug.Log("카드 뭉탱이 생성! 몬스터에게 드래그하세요.");
    }

    private void ApplySuitEffect(HandResult result)
    {
        int spadeCount = 0, heartCount = 0,
            diamondCount = 0, cloverCount = 0;

        foreach (CardView cv in selectedCards)
        {
            switch (cv.cardData.suit)
            {
                case SuitType.Spade: spadeCount++; break;
                case SuitType.Heart: heartCount++; break;
                case SuitType.Diamond: diamondCount++; break;
                case SuitType.Clover: cloverCount++; break;
            }
        }

        if (spadeCount >= heartCount &&
            spadeCount >= diamondCount &&
            spadeCount >= cloverCount)
        {
            int totalDamage = result.baseDamage + (spadeCount * 5);
            Debug.Log($"♠ Attack! Total DMG: {totalDamage}");
        }
        else if (heartCount >= spadeCount &&
                 heartCount >= diamondCount &&
                 heartCount >= cloverCount)
        {
            int healAmount = heartCount * 5;
            Debug.Log($"♥ Heal! Amount: {healAmount}");
        }
        else if (diamondCount >= spadeCount &&
                 diamondCount >= heartCount &&
                 diamondCount >= cloverCount)
        {
            int goldAmount = diamondCount * 3;
            Debug.Log($"♦ Gold! Amount: {goldAmount}");
        }
        else
        {
            int buffAmount = cloverCount * 3;
            Debug.Log($"♣ Buff! Amount: {buffAmount}");
        }
    }

    public List<CardView> GetSelectedCards()
    {
        return selectedCards;
    }
}