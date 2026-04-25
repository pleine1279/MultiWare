using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEffectTarget
{
    public float maxHealth = 100f;
    public float currentHealth;

    public System.Action<float, float> OnHealthChanged;

    public List<Relic> relics = new List<Relic>(); // 유물 리스트

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // 전투 시작 유물 발동
        foreach (var relic in relics)
        {
            relic.OnBattleStart(this);
        }
    }

    public void TakeDamage(float damage)
    {
        // 무적 상태면 피해 0 (턴 내내 유지)
        if (BattleManager.Instance != null &&
            BattleManager.Instance.IsInvincible())
        {
            Debug.Log("무적! 피해 0");
            return;
        }

        // 약화 효과: 적 공격력 -20%
        if (BattleManager.Instance != null &&
            BattleManager.Instance.isWeakened)
        {
            damage *= 0.8f;
            Debug.Log($"약화 효과! 받는 피해 -20% → {damage}");
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        foreach (var relic in relics)
            relic.OnTakeDamage(this, damage);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ApplyEffect(Effect effect)
    {
        effect.Apply(this);
    }

    public void OnTurnStart()
    {
        foreach (var relic in relics)
        {
            relic.OnTurnStart(this);
        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"플레이어 HP 회복! 현재 HP: {currentHealth}");
    }
}