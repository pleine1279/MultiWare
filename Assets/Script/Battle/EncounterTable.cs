using UnityEngine;

[CreateAssetMenu(fileName = "EncounterTable", menuName = "Game/Encounter Table")]
public class EncounterTable : ScriptableObject
{
    public MonsterEncounter[] encounters;
}
