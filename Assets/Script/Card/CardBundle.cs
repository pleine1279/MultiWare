using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardBundle : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public HandResult handResult;
    public List<CardData> cardDataList;

    // 원래 위치 저장
    private Vector2 originalPosition;
    // 몬스터에 드롭됐는지 여부
    public bool isDroppedOnMonster = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            canvas = FindFirstObjectByType<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Setup(HandResult result, List<CardData> cards)
    {
        handResult = result;
        cardDataList = cards;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 원래 위치 저장
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 몬스터에 드롭됐으면 제거
        if (isDroppedOnMonster)
        {
            Destroy(gameObject);
        }
        else
        {
            // 몬스터에 드롭 안됐으면 원래 위치로 복귀
            rectTransform.anchoredPosition = originalPosition;
            Debug.Log("몬스터에게 드롭하세요!");
        }
    }
}