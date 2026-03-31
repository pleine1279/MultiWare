using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardDealAnimator : MonoBehaviour
{
    public static CardDealAnimator Instance;

    [Header("애니메이션 설정")]
    public GameObject cardBackPrefab;    // 카드 뒷면 프리팹
    public Transform deckPosition;       // 덱 위치
    public Transform handArea;           // 패 위치
    public float dealSpeed = 0.3f;       // 카드 날아오는 속도
    public float dealDelay = 0.2f;       // 카드 간 딜레이

    private void Awake()
    {
        Instance = this;
    }

    // 카드 5장 딜 애니메이션
    public IEnumerator DealCardsAnimation(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return StartCoroutine(DealOneCard());
            yield return new WaitForSeconds(dealDelay);
        }
    }

    // 카드 한 장 딜 애니메이션
    private IEnumerator DealOneCard()
    {
        // 덱 위치에 카드 뒷면 생성
        GameObject cardBack = Instantiate(cardBackPrefab, deckPosition);
        RectTransform cardRect = cardBack.GetComponent<RectTransform>();
        cardRect.anchoredPosition = Vector2.zero;

        // 목표 위치 (HandArea)
        Vector2 startPos = deckPosition.position;
        Vector2 endPos = handArea.position;

        float elapsed = 0f;

        // 덱 → 패 로 날아가는 애니메이션
        while (elapsed < dealSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dealSpeed;

            // 부드러운 이동 (easeOut)
            t = 1 - Mathf.Pow(1 - t, 3);

            cardBack.transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        // 애니메이션 완료 후 카드 뒷면 제거
        Destroy(cardBack);

        // 실제 카드 데이터 드로우
        CardData card = DeckManager.Instance.DrawCard();
        HandManager.Instance.AddCardToHand(card);
    }
}