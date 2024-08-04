using Deck.Extensions;
using Deck.Deck.Card;

namespace GameOne.Extensions;

internal sealed class CardComparison : IComparer<CardSubType>
{
	public int Compare(CardSubType _x, CardSubType _y)
	{
		return Math.Clamp(_x.MapToScore() - _y.MapToScore(), -1, 1);
	}
}
