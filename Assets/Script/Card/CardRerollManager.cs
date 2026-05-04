using UnityEngine;

public class CardRerollManager : MonoBehaviour
{
    public static CardRerollManager Instance;

    [Header("리롤 설정")]
    public int rerollCost = 5;  // 장당 리롤 비용

    private void Awake()
    {
        Instance = this;
    }
}