using UnityEngine;

public class CardDrawer : MonoBehaviour
{
    public static CardDrawer Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DrawStartingHand(); 
    }


    // 시작할 때 카드 5장 드로우
    public void DrawStartingHand()
    {
        DrawMultipleCards(5);
    }

    // 카드 여러 장 드로우
    public void DrawMultipleCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DrawOneCard();
        }
    }

    // 카드 한 장 드로우
    public void DrawOneCard()
    {
        if (HandManager.Instance.GetHandCount() >= 5)
        {
            Debug.Log("패가 가득 찼습니다!");
            return;
        }

        CardData card = DeckManager.Instance.DrawCard();
        HandManager.Instance.AddCardToHand(card);
    }
}