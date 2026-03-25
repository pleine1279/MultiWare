using UnityEngine;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour, IPointerClickHandler
{
    private CardDrag card;

    void Awake()
    {
        card = GetComponent<CardDrag>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        card.ToggleSelect();
    }
}