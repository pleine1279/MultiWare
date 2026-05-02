// MonsterAction.cs
using UnityEngine;

public enum ActionType { Attack, Defend }

[System.Serializable]
public class MonsterAction
{
    public ActionType actionType;
    public float value;

    [Header("≈œ ∫Ò∑  ∞≠»≠")]
    public bool scaleWithTurn = false;
    public float scaleAmount = 0f;

    public float GetValue(int turnCount)
    {
        if (scaleWithTurn)
            return value + (scaleAmount * turnCount);
        return value;
    }
}