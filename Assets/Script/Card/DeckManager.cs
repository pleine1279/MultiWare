using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("덱 설정")]
    public List<CardData> allCards = new List<CardData>(); // 52장 카드 데이터

    private List<CardData> deck = new List<CardData>();    // 현재 덱

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    // 덱 초기화 (52장 복사)
    public void InitializeDeck()
    {
        deck.Clear();
        foreach (CardData card in allCards)
        {
            deck.Add(card);
        }
        Debug.Log($"덱 초기화 완료: {deck.Count}장");
    }

    // 덱 셔플
    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            CardData temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Debug.Log("덱 셔플 완료");
    }

    // 카드 한 장 뽑기
    public CardData DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.Log("덱이 비었습니다! 다시 셔플합니다.");
            InitializeDeck();
            ShuffleDeck();
        }

        CardData drawnCard = deck[0];
        deck.RemoveAt(0);
        Debug.Log($"카드 드로우: {drawnCard.cardName} (남은 덱: {deck.Count}장)");
        return drawnCard;
    }

    // 현재 덱 카드 수 반환
    public int GetDeckCount()
    {
        return deck.Count;
    }
}