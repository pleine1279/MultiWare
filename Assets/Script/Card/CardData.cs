using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card/CardData")]
public class CardData : ScriptableObject           // 핵심 카드 데이터
{
    [Header("기본 정보")]
    public string cardName;
    public int cardNumber;    // 1~13 (A~K)
    public SuitType suit;

    [Header("전투 수치")]
    public int attackPower;   // ♠ 공격력
    public int healAmount;    // ♥ 회복량
    public int goldAmount;    // ♦ 골드
    public int buffValue;     // ♣ 버프/디버프

    [Header("비주얼")]
    public Sprite cardSprite;
}