using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour, IDropHandler, IEffectTarget
{
    public MonsterData Data;
    private int currentHP;

    void Awake()
    {
        currentHP = Data.maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= (int)damage;
        Debug.Log($"몬스터 HP: {currentHP}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 카드 뭉탱이 드롭 처리
        CardBundle bundle = eventData.pointerDrag?.GetComponent<CardBundle>();
        if (bundle != null)
        {
            Debug.Log($"뭉탱이 드롭! 족보: {bundle.handResult.GetRankName()}");

            int totalDamage = CalculateSuitBonus(
                bundle.cardDataList,
                bundle.handResult.baseDamage
            );

            TakeDamage(totalDamage);
            Debug.Log($"몬스터에게 {totalDamage} 데미지!");

            Destroy(bundle.gameObject);

            if (currentHP <= 0)
                Die();
            return;
        }

        // 기존 카드 드래그 드롭 처리
        CardDrag card = eventData.pointerDrag?.GetComponent<CardDrag>();
        if (card != null)
        {
            Debug.Log("몬스터 위에 카드 드롭됨: " + gameObject.name);

            List<CardDrag> selectedGroup =
                CardSelectionManager.Instance.selectedCards;

            if (selectedGroup.Count == 0)
                selectedGroup = new List<CardDrag> { card };

            List<CardData> cardDataList = new List<CardData>();
            foreach (var c in selectedGroup)
            {
                CardView view = c.GetCardView();
                if (view != null && view.cardData != null)
                    cardDataList.Add(view.cardData);
            }

            if (cardDataList.Count > 0)
            {
                HandResult result = HandEvaluator.Evaluate(cardDataList);
                Debug.Log($"족보: {result.GetRankName()} / 기본 데미지: {result.baseDamage}");

                int totalDamage = CalculateSuitBonus(
                    cardDataList, result.baseDamage);

                TakeDamage(totalDamage);
                Debug.Log($"몬스터에게 {totalDamage} 데미지!");
            }
            else
            {
                TakeDamage(10);
                Debug.Log("기본 데미지 10 적용!");
            }

            foreach (var c in selectedGroup)
                Destroy(c.gameObject);

            CardSelectionManager.Instance.Clear();

            if (currentHP <= 0)
                Die();
        }
    }

    int CalculateSuitBonus(List<CardData> cards, int baseDamage)
    {
        int spadeCount = 0, heartCount = 0,
            diamondCount = 0, cloverCount = 0;

        foreach (CardData c in cards)
        {
            switch (c.suit)
            {
                case SuitType.Spade: spadeCount++; break;
                case SuitType.Heart: heartCount++; break;
                case SuitType.Diamond: diamondCount++; break;
                case SuitType.Clover: cloverCount++; break;
            }
        }

        if (spadeCount >= heartCount &&
            spadeCount >= diamondCount &&
            spadeCount >= cloverCount)
        {
            int damage = baseDamage + (spadeCount * 5);
            Debug.Log($"♠ Attack! Total DMG: {damage}");
            return damage;
        }
        else if (heartCount >= spadeCount &&
                 heartCount >= diamondCount &&
                 heartCount >= cloverCount)
        {
            Debug.Log($"♥ Heal! {heartCount * 5} HP");
            return 0;
        }
        else if (diamondCount >= spadeCount &&
                 diamondCount >= heartCount &&
                 diamondCount >= cloverCount)
        {
            Debug.Log($"♦ Gold! {diamondCount * 3}");
            return baseDamage;
        }
        else
        {
            Debug.Log($"♣ Buff! {cloverCount * 3}");
            return baseDamage;
        }
    }

    void Die()
    {
        Debug.Log("몬스터 사망!");
        Destroy(gameObject);
    }
}