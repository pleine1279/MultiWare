using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Relic/RemoveSpecificNumbers")]
public class RemoveSpecificNumbersRelic : Relic
{
    [Header("제거할 카드 숫자 (1=A, 11=J, 12=Q, 13=K)")]
    public List<int> numbersToRemove = new List<int>();

    public override void OnRelicAcquired(Player player, DeckManager deck)
    {
        deck.RemoveCardsByNumber(numbersToRemove);
        Debug.Log($"[{relicName}] {string.Join(", ", numbersToRemove)} 제거 완료");
    }
}