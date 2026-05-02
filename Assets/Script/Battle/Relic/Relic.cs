using UnityEngine;

public abstract class Relic : ScriptableObject
{
    public string relicName;
    public string description;

    // 전투 시작 시
    public virtual void OnBattleStart(Player player) { }

    // 턴 시작 시
    public virtual void OnTurnStart(Player player) { }

    // 공격 받을 때
    public virtual void OnTakeDamage(Player player, float damage) { }

    // 공격할 때
    public virtual void OnAttack(Player player) { }
}