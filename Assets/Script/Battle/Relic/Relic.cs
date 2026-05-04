using System.Collections.Generic;
using UnityEngine;

public abstract class Relic : ScriptableObject
{
    public string relicName;
    public string description;

    public virtual void OnBattleStart(Player player) { } // 전투 시작 시
    public virtual void OnTurnStart(Player player) { } // 턴 시작 시
    public virtual void OnTakeDamage(Player player, float damage) { } // 데미지를 받을 시
    public virtual void OnAttack(Player player, List<CardData> usedCards, Monster target, HandResult handResult) { }
    public virtual void OnRelicAcquired(Player player, DeckManager deck) { }  // 유물 획득 시 (덱 조작)
    public virtual void OnDeckShuffled(Player player, DeckManager deck) { }   // 셔플 시
}