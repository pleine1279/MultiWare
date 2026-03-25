using System.Collections.Generic;
using UnityEngine;

public class CardFan : MonoBehaviour
{
    public int cardMax = 5;
    [Header("Card Settings")]
    public GameObject cardPrefab;  // 카드 프리팹
    public int cardCount = 5;      // 초기 카드 개수
    public float spacing = 150f;   // 카드 간 간격
    public float radius = 5000f;   // 곡선 반지름
    public float maxAngle = 30f;   // 좌우 끝 카드 회전

    private GameObject[] cards;

    void Start()
    {
        //CreateCards(cardCount);
    }

    // 카드 생성
    void CreateCards(int count)
    {
        // 기존 카드 삭제
        if (cards != null)
        {
            foreach (var c in cards)
                if (c != null) Destroy(c.gameObject);
        }

        cards = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            cards[i] = card;

            // 카드에 마우스 오버 스크립트 자동 추가
            if (card.GetComponent<CardHover>() == null)
                card.AddComponent<CardHover>();
        }

        ArrangeCards();
    }

    // 카드 배치
    void ArrangeCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            float normPos = cards.Length == 1 ? 0f : (i / (float)(cards.Length - 1)) - 0.5f; // -0.5 ~ 0.5
            float x = spacing * normPos * cards.Length;
            float y = Mathf.Sqrt(Mathf.Max(0, radius * radius - x * x));

            RectTransform cardTransform = cards[i].GetComponent<RectTransform>();
            cardTransform.anchoredPosition = new Vector2(x, y - radius);
            cardTransform.rotation = Quaternion.Euler(0, 0, -normPos * maxAngle);
        }
    }

    // Inspector에서 버튼처럼 눌러 카드 추가
    [ContextMenu("Draw Card")]
    public void DrawCard()
    {
        cardCount++;
        CreateCards(cardCount);
    }

    // Inspector에서 버튼처럼 눌러 카드 제거
    [ContextMenu("Remove Card")]
    public void RemoveCard()
    {
        if (cardCount <= 1) return;
        cardCount--;
        CreateCards(cardCount);
    }
    public void RemoveCardObject(GameObject card)
    {
        if (cards == null) return;

        List<GameObject> list = new List<GameObject>(cards);
        list.Remove(card);

        cards = list.ToArray();
        cardCount = cards.Length;

        ArrangeCards();
    }
    public void DrawStartingHand()
    {
        cardCount = 5;
        CreateCards(cardCount);
    }
}