using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour        //TempCard prefab이 나중에 CardData를 참조할 수 있도록 하는 CardView.cs (3주차 카드 시스템 구현때 TempCard prefab에 붙일 스크립트)
{
    public CardData cardData;      // 이 카드의 데이터

    public Image cardImage;        // 카드 이미지
    public Text cardNameText;      // 카드 이름 텍스트

    public void Setup(CardData data)
    {
        cardData = data;

        if (data.cardSprite != null)
            cardImage.sprite = data.cardSprite;

        if (cardNameText != null)
            cardNameText.text = data.cardName;
    }
}