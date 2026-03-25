using System.Collections.Generic;
using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;

    public List<CardDrag> selectedCards = new List<CardDrag>();

    void Awake()
    {
        Instance = this;
    }

    public void Register(CardDrag card)
    {
        if (!selectedCards.Contains(card))
            selectedCards.Add(card);
    }

    public void Unregister(CardDrag card)
    {
        if (selectedCards.Contains(card))
            selectedCards.Remove(card);
    }

    public void Clear()
    {
        selectedCards.Clear();
    }

}