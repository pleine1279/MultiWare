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

        // 공격 애니메이션
        yield return new WaitForSeconds(1f);

        // 데미지 처리
        DamageEffect damage = new DamageEffect(monsterdamage);
        damage.Apply(player);

        yield return new WaitForSeconds(0.5f);

        Debug.Log("적 행동 끝");
    }
}