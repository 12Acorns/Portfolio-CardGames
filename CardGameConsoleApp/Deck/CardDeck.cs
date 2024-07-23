using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal ref struct CardDeck
{
	public CardDeck(ReadOnlySpan<Card.GameCard> _cards)
	{
		if(_cards == null || _cards.Length == 0)
		{
			throw new ArgumentException(nameof(_cards) + " is null or empty");
		}

		Cards = _cards;
	}
	public CardDeck(Card.GameCard[] _cards)
	{
		if(_cards == null || _cards.Length == 0)
		{
			throw new NullReferenceException(nameof(_cards) + " is null");
		}

		Cards = _cards;
	}

	public readonly int RemainingCards => Cards.Length - position;
	public readonly ReadOnlySpan<Card.GameCard> Cards { get; }

	private int position;


	public readonly Card.GameCard Peek()
	{
		return Current();
	}

	/// <returns>
	/// Boolean represents if not at end of card deck (true means more cards to go, false means at end)
	/// </returns>
	public bool TryNext(out Card.GameCard _card)
	{
		if(position >= Cards.Length)
		{
			_card = Cards[^1];
			return false;
		}
		_card = Current();
		position++;
		return true;
	}

	private readonly Card.GameCard Current()
	{
		var _position = position >= Cards.Length ? Cards.Length - 1 : position;
		return Cards[_position];
	}
}