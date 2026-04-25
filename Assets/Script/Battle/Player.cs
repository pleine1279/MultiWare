using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEffectTarget
{
    public static Player Instance;
    public float maxHealth = 100f;
    public float currentHealth;
    public int gold = 0;
    public System.Action<float, float> OnHealthChanged;
    public System.Action<int> OnGoldChanged;
    public List<Relic> relics = new List<Relic>(); // 유물 리스트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 안 됨
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnGoldChanged?.Invoke(gold);
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
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"플레이어 HP 회복! 현재 HP: {currentHealth}");
    }
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"골드 추가 +{amount} (현재 골드: {gold})");
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
        {
            Debug.Log("골드 부족!");
            return false;
        }

        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }

}