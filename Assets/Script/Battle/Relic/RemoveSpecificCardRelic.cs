using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Relic/RemoveSpecificCard")]
public class RemoveSpecificCardRelic : Relic
{
    [System.Serializable]
    public class CardToRemove
    {
        public SuitType suit;
        public int number;
    }

    [Header("제거할 카드 목록 (1=A, 11=J, 12=Q, 13=K)")]
    public List<CardToRemove> cardsToRemove = new List<CardToRemove>();

    public override void OnRelicAcquired(Player player, DeckManager deck)
    {
        foreach (var card in cardsToRemove)
        {
            deck.RemoveCardBySuitAndNumber(card.suit, card.number);
            Debug.Log($"[{relicName}] {card.suit} {card.number} 제거 완료");
        }
    }
}