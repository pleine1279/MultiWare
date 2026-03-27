using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("카드 데이터")]
    public CardData cardData;

    [Header("UI 연결")]
    public Image cardImage;
    public Text cardNameText;

    public void Setup(CardData data)
    {
        cardData = data;

        if (data.cardSprite != null)
            cardImage.sprite = data.cardSprite;

        if (cardNameText != null)
            cardNameText.text = data.cardName;
    }

    // CardTurn.cs의 OnClickCard()와 연결될 부분
    public SuitType GetSuit()
    {
        return cardData.suit;
    }

    public int GetAttack()
    {
        return cardData.attackPower;
    }

    public int GetHeal()
    {
        return cardData.healAmount;
    }
}