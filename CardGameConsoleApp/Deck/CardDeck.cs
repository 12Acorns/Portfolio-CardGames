using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal readonly ref struct CardDeck
{
	public CardDeck(ICard[] _cards)
	{
		if(_cards == null)
		{
			throw new NullReferenceException(nameof(_cards) + " is null");
		}

		Cards = _cards.AsSpan();
	}

	public readonly ReadOnlySpan<ICard> Cards;
}
