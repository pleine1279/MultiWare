using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    [Header("카드 데이터")]
    public CardData cardData;

    [Header("UI 연결")]
    public Image cardImage;
    public TMP_Text cardNameText;

    [Header("선택 상태")]
    public bool isSelected = false;
    private RectTransform cardContainer;
    private Vector2 containerOriginalPos;

    public void Setup(CardData data)
    {
        cardData = data;

        // CardContainer 찾기
        Transform container = transform.Find("CardContainer");
        if (container != null)
        {
            cardContainer = container.GetComponent<RectTransform>();
            containerOriginalPos = cardContainer.anchoredPosition;
        }

        // CardImage 찾기
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.gameObject.name == "CardImage")
            {
                cardImage = img;
                break;
            }
        }

        if (cardImage != null && data.cardSprite != null)
        {
            cardImage.sprite = data.cardSprite;
            cardImage.color = Color.white;
            Debug.Log($"이미지 설정 완료: {data.cardName}");
        }
        else
        {
            Debug.Log($"이미지 설정 실패 - cardImage: {cardImage}, sprite: {data.cardSprite}");
        }

        // CardNameText 찾기
        if (cardNameText == null)
            cardNameText = GetComponentInChildren<TMP_Text>();

        if (cardNameText != null)
            cardNameText.text = data.cardName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleSelect();
    }

    public void ToggleSelect()
    {
        isSelected = !isSelected;

        if (cardContainer == null) return;

        if (isSelected)
        {
            // CardContainer만 위로 올림
            cardContainer.anchoredPosition = new Vector2(
                containerOriginalPos.x,
                containerOriginalPos.y + 30
            );
            Debug.Log($"{cardData.cardName} 선택됨");
        }
        else
        {
            // CardContainer 원래 위치로
            cardContainer.anchoredPosition = containerOriginalPos;
            Debug.Log($"{cardData.cardName} 선택 해제");
        }
    }

    public void UseCard()
    {
        if (cardData == null) return;

        switch (cardData.suit)
        {
            case SuitType.Spade:
                Debug.Log($"공격! {cardData.attackPower} 데미지");
                break;
            case SuitType.Heart:
                Debug.Log($"회복! {cardData.healAmount} HP 회복");
                break;
            case SuitType.Diamond:
                Debug.Log($"골드 {cardData.goldAmount} 획득");
                break;
            case SuitType.Clover:
                Debug.Log($"버프/디버프 {cardData.buffValue} 적용");
                break;
        }

        HandManager.Instance.RemoveCardFromHand(this.gameObject);
    }

    public SuitType GetSuit() { return cardData.suit; }
    public int GetAttack() { return cardData.attackPower; }
    public int GetHeal() { return cardData.healAmount; }
    public int GetGold() { return cardData.goldAmount; }
    public int GetBuff() { return cardData.buffValue; }
}