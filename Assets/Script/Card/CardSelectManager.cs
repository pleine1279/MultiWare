using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSelectManager : MonoBehaviour
{
    public static CardSelectManager Instance;

    [Header("선택 설정")]
    public int maxSelectCount = 5;    // 최대 선택 가능 카드 수

    [Header("UI 연결")]
    public Button useCardButton;      // 카드 사용 버튼
    public TMP_Text handResultText;   // 족보 결과 텍스트

    private List<CardView> selectedCards = new List<CardView>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    // 카드 선택 추가
    public void SelectCard(CardView card)
    {
        if (selectedCards.Count >= maxSelectCount)
        {
            Debug.Log("최대 5장까지 선택 가능합니다!");
            return;
        }

        selectedCards.Add(card);
        Debug.Log($"선택된 카드: {card.cardData.cardName} ({selectedCards.Count}장)");
        UpdateUI();
    }

    // 카드 선택 해제
    public void DeselectCard(CardView card)
    {
        selectedCards.Remove(card);
        Debug.Log($"선택 해제: {card.cardData.cardName} ({selectedCards.Count}장)");
        UpdateUI();
    }

    // UI 업데이트 (족보 미리보기)
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

        // 족보 판정
        List<CardData> cardDataList = new List<CardData>();
        foreach (CardView cv in selectedCards)
            cardDataList.Add(cv.cardData);

        HandResult result = HandEvaluator.Evaluate(cardDataList);

        // 족보 텍스트 업데이트
        if (handResultText != null)
            handResultText.text = $"{result.GetRankName()} (Damage: {result.baseDamage})";

        // 사용 버튼 활성화
        if (useCardButton != null)
            useCardButton.interactable = true;

        Debug.Log($"Now: {result.GetRankName()} / Damage: {result.baseDamage}");
    }

    // 카드 사용 버튼 클릭
    public void OnClickUseCard()
    {
        if (selectedCards.Count == 0) return;

        // 족보 판정
        List<CardData> cardDataList = new List<CardData>();
        foreach (CardView cv in selectedCards)
            cardDataList.Add(cv.cardData);

        HandResult result = HandEvaluator.Evaluate(cardDataList);

        Debug.Log($"족보 사용! {result.GetRankName()} / 데미지: {result.baseDamage}");

        // 문양 시너지 계산
        ApplySuitEffect(result);

        // 선택한 카드 제거
        List<CardView> cardsToRemove = new List<CardView>(selectedCards);
        foreach (CardView cv in cardsToRemove)
        {
            HandManager.Instance.RemoveCardFromHand(cv.gameObject);
        }
        selectedCards.Clear();

        // 새 카드 드로우
        int drawCount = cardsToRemove.Count;
        StartCoroutine(CardDealAnimator.Instance.DealCardsAnimation(drawCount));

        UpdateUI();
    }

    // 문양 시너지 효과 적용
    private void ApplySuitEffect(HandResult result)
    {
        // 문양별 카드 수 계산
        int spadeCount = 0, heartCount = 0, diamondCount = 0, cloverCount = 0;

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

        // 가장 많은 문양 적용
        if (spadeCount >= heartCount && spadeCount >= diamondCount && spadeCount >= cloverCount)
        {
            int totalDamage = result.baseDamage + (spadeCount * 5);
            Debug.Log($"♠ 공격! 총 데미지: {totalDamage}");
        }
        else if (heartCount >= spadeCount && heartCount >= diamondCount && heartCount >= cloverCount)
        {
            int healAmount = heartCount * 5;
            Debug.Log($"♥ 회복! 회복량: {healAmount}");
        }
        else if (diamondCount >= spadeCount && diamondCount >= heartCount && diamondCount >= cloverCount)
        {
            int goldAmount = diamondCount * 3;
            Debug.Log($"♦ 골드 획득! 골드: {goldAmount}");
        }
        else
        {
            int buffAmount = cloverCount * 3;
            Debug.Log($"♣ 버프 적용! 버프: {buffAmount}");
        }
    }

    // 선택된 카드 목록 반환
    public List<CardView> GetSelectedCards()
    {
        return selectedCards;
    }
}