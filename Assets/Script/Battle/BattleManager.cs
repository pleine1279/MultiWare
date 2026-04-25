using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("플레이어 버프 상태")]
    public bool isFocused = false;      // 집중: 다음 족보 데미지 +40%
    public bool isInvincible = false;   // 무적: 이번 턴 받는 피해 0

    [Header("적 디버프 상태")]
    public bool isWeakened = false;     // 약화: 적 공격력 -20%
    public int weakenedTurns = 0;       // 약화 지속 턴

    public bool isExposed = false;      // 노출: 적 방어력 -20%
    public int exposedTurns = 0;        // 노출 지속 턴

    public bool isPanicked = false;     // 공황: 적 랜덤 행동
    public bool isSlowed = false;       // 둔화: 적 행동 1턴 지연

    private void Awake()
    {
        Instance = this;
    }

    // 집중 효과 적용
    public void ApplyFocus()
    {
        isFocused = true;
        Debug.Log("집중 효과 적용! 다음 족보 데미지 +40%");
    }

    // 집중 효과 사용 후 해제
    public float ConsumeFocus()
    {
        if (isFocused)
        {
            isFocused = false;
            Debug.Log("집중 효과 발동! 데미지 +40%");
            return 1.4f;
        }
        return 1.0f;
    }

    // 무적 효과 적용
    public void ApplyInvincible()
    {
        isInvincible = true;
        Debug.Log("무적 효과 적용! 이번 턴 받는 피해 0");
    }

    // 무적 효과 소비
    // 무적 효과 체크 (소비하지 않음)
    public bool IsInvincible()
    {
        if (isInvincible)
        {
            Debug.Log("무적! 피해 0");
            return true;
        }
        return false;
    }

    // 약화 효과 적용
    public void ApplyWeaken(int turns)
    {
        isWeakened = true;
        weakenedTurns = turns;
        Debug.Log($"약화 적용! 적 공격력 -20% ({turns}턴)");
    }

    // 노출 효과 적용
    public void ApplyExpose(int turns)
    {
        isExposed = true;
        exposedTurns = turns;
        Debug.Log($"노출 적용! 적 방어력 -20% ({turns}턴)");
    }

    // 공황 효과 적용
    public void ApplyPanic()
    {
        isPanicked = true;
        Debug.Log("공황 적용! 적이 이번 턴 랜덤 행동");
    }

    // 둔화 효과 적용
    public void ApplySlow()
    {
        isSlowed = true;
        Debug.Log("둔화 적용! 적 행동 1턴 지연");
    }

    // 턴 종료 시 디버프 턴 감소
    public void OnTurnEnd()
    {
        if (isWeakened)
        {
            weakenedTurns--;
            if (weakenedTurns <= 0)
            {
                isWeakened = false;
                Debug.Log("약화 효과 종료!");
            }
        }

        if (isExposed)
        {
            exposedTurns--;
            if (exposedTurns <= 0)
            {
                isExposed = false;
                Debug.Log("노출 효과 종료!");
            }
        }

        isPanicked = false;
        isSlowed = false;
        isInvincible = false;
    }
}