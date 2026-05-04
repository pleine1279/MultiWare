using UnityEngine;

[CreateAssetMenu(menuName = "Relic/RemoveSuit")]
public class RemoveSuitRelic : Relic
{
    [Header("제거할 무늬")]
    public SuitType suitToRemove;

    public override void OnRelicAcquired(Player player, DeckManager deck)
    {
        deck.RemoveCardsBySuit(suitToRemove);
        Debug.Log($"[{relicName}] {suitToRemove} 무늬 전부 제거 완료");
    }
}