using System.Collections.Generic;
using System.Linq;

public class HandEvaluator
{
    // 선택한 카드들로 족보 판정
    public static HandResult Evaluate(List<CardData> selectedCards)
    {
        if (selectedCards == null || selectedCards.Count == 0)
            return new HandResult(HandRankType.HighCard,
                HandResult.GetBaseDamage(HandRankType.HighCard), selectedCards);

        // 족보 판정 (높은 순서대로 체크)
        if (IsRoyalFlush(selectedCards))
            return new HandResult(HandRankType.RoyalFlush,
                HandResult.GetBaseDamage(HandRankType.RoyalFlush), selectedCards);

        if (IsStraightFlush(selectedCards))
            return new HandResult(HandRankType.StraightFlush,
                HandResult.GetBaseDamage(HandRankType.StraightFlush), selectedCards);

        if (IsFourOfAKind(selectedCards))
            return new HandResult(HandRankType.FourOfAKind,
                HandResult.GetBaseDamage(HandRankType.FourOfAKind), selectedCards);

        if (IsFullHouse(selectedCards))
            return new HandResult(HandRankType.FullHouse,
                HandResult.GetBaseDamage(HandRankType.FullHouse), selectedCards);

        if (IsFlush(selectedCards))
            return new HandResult(HandRankType.Flush,
                HandResult.GetBaseDamage(HandRankType.Flush), selectedCards);

        if (IsStraight(selectedCards))
            return new HandResult(HandRankType.Straight,
                HandResult.GetBaseDamage(HandRankType.Straight), selectedCards);

        if (IsThreeOfAKind(selectedCards))
            return new HandResult(HandRankType.ThreeOfAKind,
                HandResult.GetBaseDamage(HandRankType.ThreeOfAKind), selectedCards);

        if (IsTwoPair(selectedCards))
            return new HandResult(HandRankType.TwoPair,
                HandResult.GetBaseDamage(HandRankType.TwoPair), selectedCards);

        if (IsOnePair(selectedCards))
            return new HandResult(HandRankType.OnePair,
                HandResult.GetBaseDamage(HandRankType.OnePair), selectedCards);

        return new HandResult(HandRankType.HighCard,
            HandResult.GetBaseDamage(HandRankType.HighCard), selectedCards);
    }

    // 로얄 플러시: 같은 문양 10, J, Q, K, A (정확히 5장)
    private static bool IsRoyalFlush(List<CardData> cards)
    {
        if (cards.Count != 5) return false;
        if (!IsFlush(cards)) return false;
        var numbers = cards.Select(c => c.cardNumber).OrderBy(n => n).ToList();
        return numbers.SequenceEqual(new List<int> { 1, 10, 11, 12, 13 });
    }

    // 스트레이트 플러시: 같은 문양 + 연속 숫자 (정확히 5장)
    private static bool IsStraightFlush(List<CardData> cards)
    {
        if (cards.Count != 5) return false;
        return IsFlush(cards) && IsStraight(cards);
    }

    // 포카드: 같은 숫자 4장 (4장 이상)
    private static bool IsFourOfAKind(List<CardData> cards)
    {
        if (cards.Count < 4) return false;
        return cards.GroupBy(c => c.cardNumber)
                    .Any(g => g.Count() >= 4);
    }

    // 풀하우스: 트리플 + 페어 (정확히 5장)
    private static bool IsFullHouse(List<CardData> cards)
    {
        if (cards.Count != 5) return false;
        var groups = cards.GroupBy(c => c.cardNumber).ToList();
        return groups.Any(g => g.Count() == 3) &&
               groups.Any(g => g.Count() == 2);
    }

    // 플러시: 같은 문양 (정확히 5장)
    private static bool IsFlush(List<CardData> cards)
    {
        if (cards.Count != 5) return false;
        return cards.All(c => c.suit == cards[0].suit);
    }

    // 스트레이트: 연속된 숫자 (정확히 5장)
    private static bool IsStraight(List<CardData> cards)
    {
        if (cards.Count != 5) return false;
        var numbers = cards.Select(c => c.cardNumber)
                          .OrderBy(n => n).ToList();
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
                return false;
        }
        return true;
    }

    // 트리플: 같은 숫자 3장 (3장 이상)
    private static bool IsThreeOfAKind(List<CardData> cards)
    {
        if (cards.Count < 3) return false;
        return cards.GroupBy(c => c.cardNumber)
                    .Any(g => g.Count() >= 3);
    }

    // 투페어: 페어 2개 (4장 이상)
    private static bool IsTwoPair(List<CardData> cards)
    {
        if (cards.Count < 4) return false;
        return cards.GroupBy(c => c.cardNumber)
                    .Count(g => g.Count() >= 2) >= 2;
    }

    // 원페어: 같은 숫자 2장 (2장 이상)
    private static bool IsOnePair(List<CardData> cards)
    {
        if (cards.Count < 2) return false;
        return cards.GroupBy(c => c.cardNumber)
                    .Any(g => g.Count() >= 2);
    }
}