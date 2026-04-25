using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour, IDropHandler, IEffectTarget
{
    public MonsterData Data;
    private int currentHP;

    void Awake()
    {
        currentHP = Data.maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= (int)damage;
        Debug.Log($"몬스터 HP: {currentHP}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardBundle bundle = eventData.pointerDrag?.GetComponent<CardBundle>();
        if (bundle != null)
        {
            Debug.Log($"뭉탱이 드롭! 족보: {bundle.handResult.GetRankName()}");

            // 최종 데미지 계산
            int finalDamage = CalculateFinalDamage(
                bundle.cardDataList,
                bundle.handResult
            );

            TakeDamage(finalDamage);
            Debug.Log($"몬스터에게 {finalDamage} 데미지!");

            // 문양 추가 효과 적용
            ApplySuitEffects(bundle.cardDataList);

            bundle.isDroppedOnMonster = true;

            if (currentHP <= 0)
                Die();
        }
    }

    // 최종 데미지 계산
    int CalculateFinalDamage(List<CardData> cards, HandResult result)
    {
        int baseDamage = result.baseDamage;

        // 집중 효과 적용
        float focusMultiplier = 1.0f;
        if (BattleManager.Instance != null)
            focusMultiplier = BattleManager.Instance.ConsumeFocus();

        baseDamage = Mathf.RoundToInt(baseDamage * focusMultiplier);

        // 노출 효과 적용 (적 방어력 -20% → 데미지 +20%)
        if (BattleManager.Instance != null && BattleManager.Instance.isExposed)
        {
            baseDamage = Mathf.RoundToInt(baseDamage * 1.2f);
            Debug.Log($"노출 효과! 데미지 +20% → {baseDamage}");
        }

        // 스페이드 비율 계산 (5장 기준)
        int spadeCount = 0;
        foreach (CardData c in cards)
            if (c.suit == SuitType.Spade) spadeCount++;

        float spadeRatio = (float)spadeCount / 5f;

        float bonusPercent = 0f;
        if (spadeRatio >= 1.0f) bonusPercent = 0.65f;
        else if (spadeRatio >= 0.8f) bonusPercent = 0.45f;
        else if (spadeRatio >= 0.6f) bonusPercent = 0.30f;
        else if (spadeRatio >= 0.4f) bonusPercent = 0.18f;
        else if (spadeRatio >= 0.2f) bonusPercent = 0.08f;

        int finalDamage = Mathf.RoundToInt(baseDamage * (1f + bonusPercent));

        if (bonusPercent > 0)
            Debug.Log($"스페이드 보너스 +{bonusPercent * 100}% / 최종 데미지: {finalDamage}");

        return finalDamage;
    }

    // 문양별 추가 효과 적용
    void ApplySuitEffects(List<CardData> cards)
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

        // 5장 기준 비율 계산
        float heartRatio = (float)heartCount / 5f;
        float diamondRatio = (float)diamondCount / 5f;
        float cloverRatio = (float)cloverCount / 5f;

        // 하트: HP 회복
        if (heartCount > 0)
        {
            float healAmount = 0f;

            if (heartRatio >= 1.0f) healAmount = 45f;
            else if (heartRatio >= 0.8f) healAmount = 30f;
            else if (heartRatio >= 0.6f) healAmount = 20f;
            else if (heartRatio >= 0.4f) healAmount = 12f;
            else if (heartRatio >= 0.2f) healAmount = 5f;

            if (healAmount > 0)
            {
                Player player = FindFirstObjectByType<Player>();
                if (player != null)
                    player.Heal(healAmount);
                Debug.Log($"하트 회복 +{healAmount} HP");
            }
        }

        // 다이아: 골드 획득
        if (diamondCount > 0)
        {
            int goldAmount = 0;

            if (diamondRatio >= 1.0f) goldAmount = 40;
            else if (diamondRatio >= 0.8f) goldAmount = 25;
            else if (diamondRatio >= 0.6f) goldAmount = 15;
            else if (diamondRatio >= 0.4f) goldAmount = 8;
            else if (diamondRatio >= 0.2f) goldAmount = 3;

            if (goldAmount > 0)
            {
                if (GoldManager.Instance != null)
                    GoldManager.Instance.AddGold(goldAmount);
                Debug.Log($"다이아 골드 +{goldAmount}G");
            }
        }

        // 클로버: 버프/디버프
        if (cloverCount > 0)
        {
            ApplyCloverEffect(cloverRatio);
        }
    }

    // 클로버 효과 적용
    void ApplyCloverEffect(float ratio)
    {
        if (ratio <= 0) return;

        // 20% → 자동 약화 디버프
        if (ratio < 0.4f)
        {
            Debug.Log("클로버 약화: 적 공격력 -20% (2턴) 자동 발동!");
            ApplyDebuff(ratio);
            return;
        }

        // 40% 이상 → 선택창 표시
        string debuffName = "", debuffDesc = "";
        string buffName = "", buffDesc = "";

        if (ratio < 0.6f) // 40%
        {
            debuffName = "Expose";
            debuffDesc = "Enemy DEF -20% (2 turns)";
            buffName = "Inspiration";
            buffDesc = "Draw +1 card now";
        }
        else if (ratio < 0.8f) // 60%
        {
            debuffName = "Panic";
            debuffDesc = "Enemy acts randomly (1 turn)";
            buffName = "Focus";
            buffDesc = "Next hand DMG +40%";
        }
        else if (ratio < 1.0f) // 80%
        {
            debuffName = "Slow";
            debuffDesc = "Enemy action delayed 1 turn";
            buffName = "Lucky";
            buffDesc = "Draw +2 cards now";
        }
        else // 100%
        {
            debuffName = "Curse";
            debuffDesc = "2 random debuffs applied";
            buffName = "Invincible";
            buffDesc = "Take 0 damage this turn";
        }

        // 선택창 표시
        if (CloverChoiceUI.Instance != null)
        {
            CloverChoiceUI.Instance.ShowChoice(
                debuffName, debuffDesc,
                buffName, buffDesc,
                () => ApplyDebuff(ratio),
                () => ApplyBuff(ratio)
            );
        }
    }

    // 디버프 적용
    void ApplyDebuff(float ratio)
    {
        if (BattleManager.Instance == null) return;

        if (ratio < 0.4f)
        {
            BattleManager.Instance.ApplyWeaken(2);
            Debug.Log("약화: 적 공격력 -20% (2턴)");
        }
        else if (ratio < 0.6f)
        {
            BattleManager.Instance.ApplyExpose(2);
            Debug.Log("노출: 적 방어력 -20% (2턴)");
        }
        else if (ratio < 0.8f)
        {
            BattleManager.Instance.ApplyPanic();
            Debug.Log("공황: 적이 이번 턴 랜덤 행동 (1턴)");
        }
        else if (ratio < 1.0f)
        {
            BattleManager.Instance.ApplySlow();
            Debug.Log("둔화: 적 행동 1턴 지연");
        }
        else
        {
            // 저주: 랜덤 디버프 2종
            int[] debuffs = { 0, 1, 2, 3 };
            System.Random rng = new System.Random();
            int first = debuffs[rng.Next(debuffs.Length)];
            int second;
            do { second = debuffs[rng.Next(debuffs.Length)]; }
            while (second == first);

            ApplyRandomDebuff(first);
            ApplyRandomDebuff(second);
            Debug.Log("저주: 랜덤 디버프 2종 동시 발동!");
        }
    }

    void ApplyRandomDebuff(int index)
    {
        if (BattleManager.Instance == null) return;
        switch (index)
        {
            case 0: BattleManager.Instance.ApplyWeaken(2); break;
            case 1: BattleManager.Instance.ApplyExpose(2); break;
            case 2: BattleManager.Instance.ApplyPanic(); break;
            case 3: BattleManager.Instance.ApplySlow(); break;
        }
    }

    void ApplyBuff(float ratio)
    {
        if (ratio < 0.6f)
        {
            Debug.Log("영감: 즉시 드로우 +1장");
            if (TurnManager.Instance != null && CardDealAnimator.Instance != null)
                TurnManager.Instance.StartCoroutine(
                    CardDealAnimator.Instance.DealCardsAnimation(1));
        }
        else if (ratio < 0.8f)
        {
            Debug.Log("집중: 다음 족보 데미지 +40%");
            if (BattleManager.Instance != null)
                BattleManager.Instance.ApplyFocus();
        }
        else if (ratio < 1.0f)
        {
            Debug.Log("천운: 즉시 드로우 +2장");
            if (TurnManager.Instance != null && CardDealAnimator.Instance != null)
                TurnManager.Instance.StartCoroutine(
                    CardDealAnimator.Instance.DealCardsAnimation(2));
        }
        else
        {
            Debug.Log("무적: 이번 턴 받는 피해 0");
            if (BattleManager.Instance != null)
                BattleManager.Instance.ApplyInvincible();
        }
    }

    void Die()
    {
        Debug.Log("몬스터 사망!");
        Destroy(gameObject);
    }
}