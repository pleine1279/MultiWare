using System.Collections.Generic;
using UnityEngine;

public class HandResult
{
    public HandRankType rank;
    public int baseDamage;
    public List<CardData> bestCards;

    public HandResult(HandRankType rank, int baseDamage, List<CardData> bestCards)
    {
        this.rank = rank;
        this.baseDamage = baseDamage;
        this.bestCards = bestCards;
    }

    public string GetRankName()
    {
        switch (rank)
        {
            case HandRankType.RoyalFlush: return "Royal Flush";
            case HandRankType.StraightFlush: return "Straight Flush";
            case HandRankType.FourOfAKind: return "Four of a Kind";
            case HandRankType.FullHouse: return "Full House";
            case HandRankType.Flush: return "Flush";
            case HandRankType.Straight: return "Straight";
            case HandRankType.ThreeOfAKind: return "Three of a Kind";
            case HandRankType.TwoPair: return "Two Pair";
            case HandRankType.OnePair: return "One Pair";
            default: return "High Card";
        }
    }

    // 족보 배율 반환
    public static float GetMultiplier(HandRankType rank)
    {
        switch (rank)
        {
            case HandRankType.RoyalFlush: return 8.0f;
            case HandRankType.StraightFlush: return 6.0f;
            case HandRankType.FourOfAKind: return 4.5f;
            case HandRankType.FullHouse: return 3.0f;
            case HandRankType.Flush: return 2.5f;
            case HandRankType.Straight: return 2.0f;
            case HandRankType.ThreeOfAKind: return 1.6f;
            case HandRankType.TwoPair: return 1.3f;
            case HandRankType.OnePair: return 1.1f;
            default: return 0.8f;
        }
    }

    // 카드 숫자 합산 점수 계산
    public static int CalculateTotalScore(List<CardData> cards)
    {
        int total = 0;
        foreach (CardData card in cards)
            total += card.GetCardScore();
        return total;
    }

    // 최종 데미지 계산 (숫자 합산 점수 × 족보 배율)
    public static int CalculateDamage(HandRankType rank, List<CardData> cards)
    {
        int totalScore = CalculateTotalScore(cards);
        float multiplier = GetMultiplier(rank);
        return Mathf.RoundToInt(totalScore * multiplier);
    }
}