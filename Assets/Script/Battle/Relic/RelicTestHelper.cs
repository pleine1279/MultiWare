using UnityEngine;

public class RelicTestHelper : MonoBehaviour
{
    public Relic testRelic;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RelicManager.Instance.AcquireRelic(testRelic);
        }
    }
}