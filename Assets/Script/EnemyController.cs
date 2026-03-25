using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public IEnumerator EnemyAction()
    {
        Debug.Log("적 공격 시작");

        // 공격 애니메이션
        yield return new WaitForSeconds(1f);

        // 데미지 처리
        Debug.Log("플레이어에게 데미지!");

        yield return new WaitForSeconds(0.5f);

        Debug.Log("적 행동 끝");
    }
}