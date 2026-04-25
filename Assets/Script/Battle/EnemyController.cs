using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Player player;
    public float monsterdamage;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        if (player == null)
            Debug.LogError($"{gameObject.name}: Player를 찾을 수 없습니다.");

        // Data는 Monster에서 직접 가져오기
        Monster monster = GetComponent<Monster>();
        if (monster != null && monster.Data != null)
            monsterdamage = monster.Data.attackDamage;
        else
            Debug.LogError($"[{gameObject.name}] Monster 또는 MonsterData가 없습니다!");
    }

    public IEnumerator EnemyAction()
    {
        Debug.Log($"[{gameObject.name}] 공격 시작!");
        
        yield return new WaitForSeconds(1f); //몬스터 공격 시간용

        DamageEffect damage = new DamageEffect(monsterdamage);
        damage.Apply(player);
        Debug.Log($"[{gameObject.name}] Player에게 {monsterdamage} 데미지!");

        yield return new WaitForSeconds(0.5f); //다음 공격까지 살짝 대기
        Debug.Log($"[{gameObject.name}] 공격 끝!");
    }
}
