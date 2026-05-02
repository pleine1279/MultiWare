using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Game/Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int maxHP;
    //public float attackDamage;
    public int gold;

    public List<MonsterAction> actionPattern; 
}