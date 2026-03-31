using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    [Header("패 설정")]
    public int maxHandSize = 5;           // 최대 패 크기
    public GameObject cardPrefab;         // 카드 프리팹
    public Transform handArea;            // 카드가 배치될 부모 오브젝트

    private List<CardData> hand = new List<CardData>();        // 현재 패
    private List<GameObject> handObjects = new List<GameObject>(); // 패 오브젝트

    private void Awake()
    {
        Instance = this;
    }

    // 카드 한 장 패에 추가
    public void AddCardToHand(CardData cardData)
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("패가 가득 찼습니다!");
            return;
        }

        hand.Add(cardData);

        // 카드 오브젝트 생성
        GameObject cardObj = Instantiate(cardPrefab, handArea);
        CardView cardView = cardObj.GetComponent<CardView>();

        if (cardView != null)
            cardView.Setup(cardData);

        handObjects.Add(cardObj);
        Debug.Log($"패에 추가: {cardData.cardName} (패: {hand.Count}장)");
    }

    // 패에서 카드 제거
    public void RemoveCardFromHand(GameObject cardObj)
    {
        int index = handObjects.IndexOf(cardObj);
        if (index >= 0)
        {
            hand.RemoveAt(index);
            handObjects.RemoveAt(index);
            Destroy(cardObj);
            Debug.Log($"카드 사용됨 (남은 패: {hand.Count}장)");
        }
    }

    // 패 전체 초기화
    public void ClearHand()
    {
        foreach (GameObject obj in handObjects)
            Destroy(obj);

        hand.Clear();
        handObjects.Clear();
        Debug.Log("패 초기화 완료");
    }

    // 현재 패 카드 수 반환
    public int GetHandCount()
    {
        return hand.Count;
    }
}