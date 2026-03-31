using UnityEngine;
using System.Collections;

public class CardDrawer : MonoBehaviour
{
    public static CardDrawer Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CardDealAnimator.Instance.DealCardsAnimation(5));
    }

    public void DrawOneCard()
    {
        StartCoroutine(CardDealAnimator.Instance.DealCardsAnimation(1));
    }
}