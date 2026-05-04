using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void AcquireRelic(Relic relic)
    {
        if (relic == null) return;

        relic.OnRelicAcquired(Player.Instance, DeckManager.Instance);
        Debug.Log($"[RelicManager] {relic.relicName} È¹µæ!");
    }
}