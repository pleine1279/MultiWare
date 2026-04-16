using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    public Vector2 originalPos;
    public Transform originalParent;
    public Vector3 originalScale;
    public Quaternion originalRotation; // 회전 추가
    public Image originalImage;

    public bool isSelected = false;

    private List<RectTransform> group = new List<RectTransform>();
    private LayoutElement layoutElement;

    private Dictionary<RectTransform, Vector2> offsets = new Dictionary<RectTransform, Vector2>();
    private RectTransform anchor;

    public float spacingX = 50f;
    public float spacingY = 0f;

    private CardView cardView;
    private bool isDraggable = false;  // ← 기본값 false

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalRotation = rectTransform.localRotation; // 초기 회전 저장
        originalImage = GetComponent<Image>();
        layoutElement = GetComponent<LayoutElement>();

        cardView = GetComponent<CardView>();
    }

    // 카드 선택 토글
    public void ToggleSelect()
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            CardSelectionManager.Instance.Register(this);

            // CardView가 있으면 CardSelectManager에도 등록
            CardView cardView = GetComponent<CardView>();
            if (cardView != null && CardSelectManager.Instance != null)
                CardSelectManager.Instance.SelectCard(cardView);
        }
        else
        {
            CardSelectionManager.Instance.Unregister(this);

            // CardView가 있으면 CardSelectManager에서도 해제
            CardView cardView = GetComponent<CardView>();
            if (cardView != null && CardSelectManager.Instance != null)
                CardSelectManager.Instance.DeselectCard(cardView);
        }
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 비활성화 상태면 무시
        if (!isDraggable) return;

        var selected = CardSelectionManager.Instance.selectedCards;

        if (selected.Count == 0 || !isSelected)
        {
            selected = new List<CardDrag> { this };
        }

        // 위치 기준 정렬
        selected.Sort((a, b) =>
            a.transform.position.x.CompareTo(b.transform.position.x));

        group.Clear();
        offsets.Clear();

        // anchor는 무조건 첫 번째 카드
        anchor = selected[0].GetComponent<RectTransform>();
        Vector2 startPos = anchor.anchoredPosition+new Vector2(0, -180f);
        for (int i = 0; i < selected.Count; i++)
        {
            var card = selected[i];
            RectTransform rt = card.GetComponent<RectTransform>();
            group.Add(rt);

            // 원래 상태 저장
            card.originalParent = card.transform.parent;
            card.originalPos = rt.anchoredPosition;
            card.originalRotation = rt.localRotation;

            rt.localRotation = Quaternion.identity;

            if (card.layoutElement != null)
                card.layoutElement.ignoreLayout = true;

            card.transform.SetParent(canvas.transform);
            card.transform.SetAsLastSibling();
            card.originalImage.raycastTarget = false;
            card.rectTransform.localScale = card.originalScale * 1.1f;

            // offset을 index 기준으로만
            Vector2 offset = new Vector2(i * spacingX, i * spacingY);
            offsets[rt] = offset;
            // 겹침 방지 (무조건 재배치)
            rt.anchoredPosition = startPos + offset;
        }
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        // anchor 이동
        anchor.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // 나머지는 offset 유지
        foreach (var rt in group)
        {
            if (rt == anchor) continue;
            rt.anchoredPosition = anchor.anchoredPosition + offsets[rt];
        }
    }

    // 드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        var selected = CardSelectionManager.Instance.selectedCards;

        if (selected.Count == 0 || !isSelected)
        {
            selected = new List<CardDrag> { this };
        }

        foreach (var card in selected)
        {
            card.ResetCard();
        }
    }

    // 카드 초기 상태로 복구
    public void ResetCard()
    {
        // 크기 복원
        rectTransform.localScale = originalScale;

        // 회전 복원
        rectTransform.localRotation = originalRotation;

        // 레이캐스트 활성화
        originalImage.raycastTarget = true;

        // 부모 및 위치 복구
        if (transform.parent == canvas.transform && originalParent != null)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPos;

            if (layoutElement != null)
                layoutElement.ignoreLayout = false;
        }
    }

    public CardView GetCardView()
    {
        return cardView;
    }
    // 드래그 활성화
    public void SetDraggable(bool draggable)
    {
        isDraggable = draggable;
    }
}