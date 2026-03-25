using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public GameState currentState;
    public CardFan cardFan;
    [Header("UI")]
    public CanvasGroup inputBlocker;
    public Button endTurnButton;

    [Header("Enemy")]
    public EnemyController enemy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartPlayerTurn();
    }

    // 턴 종료 버튼
    public void OnClickEndTurn()
    {
        if (currentState != GameState.PlayerTurn)
            return;

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        currentState = GameState.Busy;

        // 입력 막기
        SetInputBlock(true);
        endTurnButton.interactable = false;

        Debug.Log("적 턴 시작");

        // 적 행동 실행
        yield return StartCoroutine(enemy.EnemyAction());

        Debug.Log("적 턴 종료");

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        currentState = GameState.PlayerTurn;

        // 입력 허용
        SetInputBlock(false);
        endTurnButton.interactable = true;

        cardFan.DrawStartingHand();

        Debug.Log("플레이어 턴 시작");
    }

    void SetInputBlock(bool block)
    {
        inputBlocker.blocksRaycasts = block;
        inputBlocker.interactable = !block;
    }
}