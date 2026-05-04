using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour, IDropHandler, IEffectTarget
{
    public MonsterData Data;
    private int currentHP;
    private int monsterGold;
    private int defense = 0;
    void Awake() 
    {
        // Data가 이미 있으면 초기화 (인스펙터에서 직접 할당한 경우)
        if (Data != null)
        {
            currentHP = Data.maxHP;
            monsterGold = Data.gold;
        }
    }
    public void AddDefense(int amount)
    {
        defense += amount;
        Debug.Log($"[{gameObject.name}] 방어력 {defense} 획득!");
    }
    public void Initialize(MonsterData data)
    {
        Data = data;
        currentHP = data.maxHP;
        monsterGold = data.gold;
    }

    public void TakeDamage(float damage)
    {
        int finalDamage = Mathf.Max(0, (int)damage - defense);
        currentHP -= finalDamage;
        defense = 0; // 방어력 턴마다 초기화
        Debug.Log($"[{gameObject.name}] {finalDamage} 데미지! HP: {currentHP}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardBundle bundle = eventData.pointerDrag?.GetComponent<CardBundle>();
        if (bundle != null)
        {
            Debug.Log($"뭉탱이 드롭! 족보: {bundle.handResult.GetRankName()}");

            int totalDamage = CalculateSuitBonus(
                bundle.cardDataList,
                bundle.handResult.baseDamage
            );
            foreach (var relic in Player.Instance.relics)
            {
                relic.OnAttack(Player.Instance, bundle.cardDataList, this, bundle.handResult);
            }

            TakeDamage(totalDamage);
            Debug.Log($"몬스터에게 {totalDamage} 데미지!");

            // 몬스터에 드롭됐음을 표시
            bundle.isDroppedOnMonster = true;

            if (currentHP <= 0)
                Die();
        }
    }

    int CalculateSuitBonus(List<CardData> cards, int baseDamage)
    {
        int spadeCount = 0, heartCount = 0,
            diamondCount = 0, cloverCount = 0;

        foreach (CardData c in cards)
        {
            switch (c.suit)
            {
                case SuitType.Spade: spadeCount++; break;
                case SuitType.Heart: heartCount++; break;
                case SuitType.Diamond: diamondCount++; break;
                case SuitType.Clover: cloverCount++; break;
            }
        }

        // 족보 데미지는 항상 적용
        int totalDamage = baseDamage;

        // 가장 많은 문양 추가 효과 적용
        if (spadeCount >= heartCount &&
            spadeCount >= diamondCount &&
            spadeCount >= cloverCount)
        {
            // 스페이드: 추가 데미지
            totalDamage += spadeCount * 5;
            Debug.Log($"♠ 추가 공격! +{spadeCount * 5} / 총 데미지: {totalDamage}");
        }
        else if (heartCount >= spadeCount &&
                 heartCount >= diamondCount &&
                 heartCount >= cloverCount)
        {
            // 하트: 회복 + 기본 족보 데미지는 유지
            float healAmount = heartCount * 5;
            Player player = FindFirstObjectByType<Player>();
            if (player != null)
                player.Heal(healAmount);
            Debug.Log($"♥ 회복! +{healAmount} HP / 족보 데미지: {totalDamage}");
        }
        else if (diamondCount >= spadeCount &&
                 diamondCount >= heartCount &&
                 diamondCount >= cloverCount)
        {
            // 다이아: 골드 획득
            int goldAmount = diamondCount * 3;
            Debug.Log($"♦ 골드 획득! +{goldAmount}");
        }
        else
        {
            // 클로버: 버프
            int buffAmount = cloverCount * 3;
            Debug.Log($"♣ 버프! +{buffAmount}");
        }

        return totalDamage;
    }
    void Die()
    {
        Debug.Log("몬스터 사망!");
        SpawnManager.Instance.OnMonsterDied(this);
        Destroy(gameObject);
        Player.Instance.AddGold(monsterGold);
    }

}