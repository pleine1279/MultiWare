using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Relic/BonusByHandRank")]
public class BonusByHandRankRelic : Relic
{
    [Header("발동 족보")]
    public List<HandRankType> triggerRanks; // 발동할 족보 선택

    [Header("효과")]
    public int bonusDamage;
    public int bonusGold;

    public override void OnAttack(Player player, List<CardData> usedCards, Monster target, HandResult handResult)
    {
        if (!triggerRanks.Contains(handResult.rank)) return;

        if (bonusDamage > 0)
        {
            target.TakeDamage(bonusDamage);
            Debug.Log($"[{relicName}] {handResult.GetRankName()} 발동! 추가 데미지 {bonusDamage}");
        }

        if (bonusGold > 0)
        {
            player.AddGold(bonusGold);
            Debug.Log($"[{relicName}] 골드 {bonusGold} 획득!");
        }
    }
}