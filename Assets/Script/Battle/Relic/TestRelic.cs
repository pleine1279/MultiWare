using UnityEngine;

[CreateAssetMenu(fileName = "TestRelic", menuName = "Relic/TestRelic", order = 1)]
public class TestRelic : Relic
{
    public override void OnTakeDamage(Player player, float damage)
    {
        Debug.Log($"[유물 발동] 데미지 받음: {damage}");
    }

    public override void OnBattleStart(Player player)
    {
        Debug.Log("[유물 발동] 전투 시작");
    }

    public override void OnTurnStart(Player player)
    {
        Debug.Log("[유물 발동] 턴 시작");
    }
}