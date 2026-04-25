using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public GameState currentState;
    public CardFan cardFan;

    [Header("UI")]
    public CanvasGroup inputBlocker;
    public Button endTurnButton;
    public Player player;

    [Header("Enemy")]
    private EnemyController[] enemies;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        StartPlayerTurn();
    }

    // 턴 종료 버튼
    public void OnClickEndTurn()
    {
        if (currentState != GameState.PlayerTurn)
            return;

        // 남아있는 CardBundle 확인
        CardBundle[] bundles = FindObjectsByType<CardBundle>(FindObjectsSortMode.None);
        if (bundles.Length > 0)
        {
            Debug.Log("몬스터에게 카드를 먼저 사용하세요!");

            // UI 메시지 표시
            if (CardSelectManager.Instance != null)
                CardSelectManager.Instance.ShowMessage("Use card on monster first!");
            return;
        }

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        currentState = GameState.EnemyTurn;
        SetInputBlock(true);
        endTurnButton.interactable = false;

        Debug.Log("적 턴 시작");

        // 둔화 효과 먼저 체크 (OnTurnEnd 전에!)
        if (BattleManager.Instance != null && BattleManager.Instance.isSlowed)
        {
            Debug.Log("둔화 효과! 적 행동 1턴 지연");
            BattleManager.Instance.isSlowed = false;
        }
        else
        {
            foreach (EnemyController enemy in enemies)
            {
                if (enemy != null)
                    yield return StartCoroutine(enemy.EnemyAction());
            }
        }

        // 적 행동 끝난 후 턴 종료 처리
        if (BattleManager.Instance != null)
            BattleManager.Instance.OnTurnEnd();

        Debug.Log("적 턴 종료");
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        currentState = GameState.PlayerTurn;
        player.OnTurnStart();
        SetInputBlock(false);
        endTurnButton.interactable = true;

        // 현재 손에 있는 카드 수 확인
        int currentCount = cardFan.GetCurrentCardCount();
        int drawCount = 5 - currentCount;

        Debug.Log($"현재 카드: {currentCount}장 / 드로우할 카드: {drawCount}장");

        if (drawCount > 0)
        {
            if (CardDealAnimator.Instance != null)
                StartCoroutine(CardDealAnimator.Instance.DealCardsAnimation(drawCount));
            else
                cardFan.DrawStartingHand();
        }

        Debug.Log("플레이어 턴 시작");
    }

    void SetInputBlock(bool block)
    {
        inputBlocker.blocksRaycasts = block;
        inputBlocker.interactable = !block;
    }
}