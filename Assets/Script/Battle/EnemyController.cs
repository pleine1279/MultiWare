using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Player player;
    private MonsterData data;
    private int patternIndex = 0; // 현재 패턴 인덱스

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        if (player == null)
            Debug.LogError($"[{gameObject.name}] Player를 찾을 수 없습니다!");

        Monster monster = GetComponent<Monster>();
        if (monster != null && monster.Data != null)
            data = monster.Data;
        else
            Debug.LogError($"[{gameObject.name}] MonsterData가 없습니다!");
    }

    public IEnumerator EnemyAction()
    {
        if (data == null || data.actionPattern.Count == 0)
        {
            Debug.LogError($"[{gameObject.name}] 패턴이 없습니다!");
            yield break;
        }

        // 현재 턴 패턴 가져오기
        MonsterAction action = data.actionPattern[patternIndex];

        switch (action.actionType)
        {
            case ActionType.Attack:
                yield return StartCoroutine(DoAttack(action.value));
                break;

            case ActionType.Defend:
                yield return StartCoroutine(DoDefend(action.value));
                break;
        }

        // 다음 패턴으로 (끝나면 처음으로)
        patternIndex = (patternIndex + 1) % data.actionPattern.Count;
    }

    IEnumerator DoAttack(float damage)
    {
        Debug.Log($"[{gameObject.name}] 공격! {damage} 데미지");
        yield return new WaitForSeconds(1f);
        DamageEffect dmg = new DamageEffect(damage);
        dmg.Apply(player);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator DoDefend(float amount)
    {
        Debug.Log($"[{gameObject.name}] 방어! {amount} 방어력");
        yield return new WaitForSeconds(1f);
        // 방어 로직 (Player의 다음 공격 데미지 감소 등)
        GetComponent<Monster>().AddDefense((int)amount);
        yield return new WaitForSeconds(0.5f);
    }
}