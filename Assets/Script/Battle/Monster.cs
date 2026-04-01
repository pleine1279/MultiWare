using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour, IDropHandler, IEffectTarget
{
    // 몬스터 채력 및 카드로 인한 데미지
    public MonsterData Data;
    private int currentHP;

    void Awake()
    {
        currentHP = Data.maxHP;
    }
    public void TakeDamage(float damage)
    {
        currentHP -= (int)damage;
    }
    public void OnDrop(PointerEventData eventData)
    {
        CardDrag card = eventData.pointerDrag?.GetComponent<CardDrag>();

        if (card != null)
        {
            Debug.Log("몬스터 위에 카드 드롭됨: " + gameObject.name);

            // 선택된 카드 그룹 가져오기
            List<CardDrag> selectedGroup = CardSelectionManager.Instance.selectedCards;

            // 그룹 전체 처리
            if (selectedGroup.Count == 0)
            {
                // 선택된 카드가 없으면 드래그된 카드 하나만 처리
                selectedGroup = new List<CardDrag> { card };
            }

            foreach (var c in selectedGroup)
            {
                // 카드 효과 처리
                ReceiveCard(c);

                // 카드 제거
                Destroy(c.gameObject);
            }

            // 선택 카드 초기화
            CardSelectionManager.Instance.Clear();
        }
    }
    void ReceiveCard(CardDrag card)
    {
        //여기는 확장 안된 상태 카드 구현되면 해야함
        // HP 감소
        currentHP--;

        Debug.Log($"몬스터 HP 감소! 현재 HP: {currentHP}");

        // 0 이하이면 제거
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("몬스터 사망!");
        Destroy(gameObject);
    }
}