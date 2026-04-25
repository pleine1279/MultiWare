using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Game/Monster Encounter")]
public class MonsterEncounter : ScriptableObject
{
    public string encounterName;

    [Tooltip("MonsterData를 가진 프리팹 등록. 빈 슬롯은 None으로")]
    public GameObject[] monsterPrefabSlots = new GameObject[3]; // 프리팹 직접 등록
}
