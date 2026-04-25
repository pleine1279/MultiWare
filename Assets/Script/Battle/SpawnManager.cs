using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("컴포넌트 연결")]
    public MonsterSpawner spawner;

    private List<Monster> aliveMonsters = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spawner.SpawnFromEncounterTable();

        // 소환된 몬스터 추적 목록 초기화
        aliveMonsters.Clear();
        foreach (var obj in spawner.spawnedMonsters)
        {
            if (obj == null) continue;
            Monster m = obj.GetComponent<Monster>();
            if (m != null) aliveMonsters.Add(m);
        }
    }

    public void OnMonsterDied(Monster monster)
    {
        aliveMonsters.Remove(monster);

        if (aliveMonsters.Count == 0)
            GameManager.Instance.OnVictory();
    }

    public void OnPlayerDead()
    {
        GameManager.Instance.OnDefeat();
    }
}
