using System.Collections.Generic;

public class HandResult
{
    public HandRankType rank;        // 족보 종류
    public int baseDamage;           // 기본 데미지
    public List<CardData> bestCards; // 족보를 구성하는 카드들

    public HandResult(HandRankType rank, int baseDamage, List<CardData> bestCards)
    {
        this.rank = rank;
        this.baseDamage = baseDamage;
        this.bestCards = bestCards;
    }

    // 족보 이름 반환
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

    // 족보별 기본 데미지
    public static int GetBaseDamage(HandRankType rank)
    {
        switch (rank)
        {
            case HandRankType.RoyalFlush: return 100;
            case HandRankType.StraightFlush: return 80;
            case HandRankType.FourOfAKind: return 60;
            case HandRankType.FullHouse: return 50;
            case HandRankType.Flush: return 40;
            case HandRankType.Straight: return 30;
            case HandRankType.ThreeOfAKind: return 25;
            case HandRankType.TwoPair: return 20;
            case HandRankType.OnePair: return 15;
            default: return 10;
        }
    }
}