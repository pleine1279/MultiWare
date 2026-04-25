using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Player player;
    public MonsterData Data;
    public float monsterdamage;

    private void Awake()
    {
        monsterdamage = Data.attackDamage;
    }

    public IEnumerator EnemyAction()
    {
        Debug.Log("적 공격 시작");

        // 공황 상태 체크
        if (BattleManager.Instance != null && BattleManager.Instance.isPanicked)
        {
            // 50% 확률로 공격하거나 안 함
            int random = Random.Range(0, 2);

            if (random == 0)
            {
                Debug.Log("공황! 적이 혼란스러워 공격하지 못했다!");
                yield return new WaitForSeconds(1f);
            }
            else
            {
                Debug.Log("공황! 적이 혼란스럽지만 공격했다!");
                yield return new WaitForSeconds(1f);

                DamageEffect damage = new DamageEffect(monsterdamage);
                damage.Apply(player);

                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            // 일반 공격
            yield return new WaitForSeconds(1f);

            DamageEffect damage = new DamageEffect(monsterdamage);
            damage.Apply(player);

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("적 행동 끝");
    }
}