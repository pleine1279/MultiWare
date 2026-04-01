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
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 유물 반응
        foreach (var relic in relics)
        {
            relic.OnTakeDamage(this, damage);
        }

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
}