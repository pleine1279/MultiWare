using UnityEngine;

public class CardNotifier : MonoBehaviour
{
    private CardFan fan;

    void Start()
    {
        fan = GetComponentInParent<CardFan>();
    }

    void OnDestroy()
    {
        if (fan != null)
        {
            fan.RemoveCardObject(gameObject);
        }
    }
}