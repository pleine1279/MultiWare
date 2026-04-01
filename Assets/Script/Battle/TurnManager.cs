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

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        currentState = GameState.EnemyTurn;

        SetInputBlock(true);
        endTurnButton.interactable = false;

        Debug.Log("적 턴 시작");

        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
                yield return StartCoroutine(enemy.EnemyAction());
        }

        Debug.Log("적 턴 종료");

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        currentState = GameState.PlayerTurn;

        player.OnTurnStart(); // 플레이어 유물 발동

        // 입력 막기 비활성화
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