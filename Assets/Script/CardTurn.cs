using UnityEngine;

public class CardTurn : MonoBehaviour
{
    public void OnClickCard()
    {
        //플레이어 턴 아니면 사용 불가
        if (TurnManager.Instance.currentState != GameState.PlayerTurn)
        {
            Debug.Log("지금은 카드 사용 불가");
            return;
        }

        Debug.Log("카드 사용!");

        //카드 효과 실행
    }
}