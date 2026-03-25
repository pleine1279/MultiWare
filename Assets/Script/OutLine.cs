using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class CardVisual : MonoBehaviour
{
    private Outline outline;
    private CardDrag card;
    void Awake()
    {
        outline = GetComponent<Outline>();
        card = GetComponent<CardDrag>();
    }

    void Update()
    {
        outline.enabled = card.isSelected;
        if (card.isSelected)
        {
            card.transform.localScale = card.originalScale * 1.1f;
        }
        else
        {
            card.transform.localScale = card.originalScale;
        }
    }
}