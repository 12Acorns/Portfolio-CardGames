using Deck.Deck.Randomizer;
using Deck.Deck.Card;

namespace Deck.Deck;

public sealed class CardDeck
{
	public CardDeck(ReadOnlySpan<GameCard> _cards, IRandomizer _randomizer, int _maximumLength)
	{
		if(_cards.Length > _maximumLength)
		{
			throw new Exception($"{nameof(_cards)} Length must be equal to or less than {nameof(_maximumLength)}");
		}

		position = _maximumLength - 1;
		cards = new GameCard[_maximumLength];

		for(int i = _maximumLength - 1; i > _maximumLength - 1 - _cards.Length; i--)
		{
			cards[i] = _cards[_maximumLength - 1 - i];
			nextFreePosition = i - 1;
		}

		if(_cards.Length == 0)
		{
			nextFreePosition = position;
		}

		randomizer = _randomizer;
	}

	public int RemainingCards => cards.Length - (position + 1);
	public ReadOnlySpan<GameCard> Cards => cards;

	private int position;
	private int nextFreePosition;
	private readonly GameCard[] cards;
	private readonly IRandomizer randomizer;

	public void Shuffle(IRandomizer _randomizer)
	{
		position = cards.Length - 1;
		_randomizer.Randomize(cards);
	}
	public void Shuffle()
	{
		position = cards.Length - 1;
		randomizer.Randomize(cards);
	}

	public void SetCurrent(GameCard _card)
	{
		cards[position] = _card;
	}
	public GameCard Peek()
	{
		var _curent = Current();
		return _curent;
	}
	public bool AddRange(ReadOnlySpan<GameCard> _cards)
	{
		int _length = _cards.Length;

		if(nextFreePosition - _length < 0)
		{
			return false;
		}

		for(int i = 0; i < _length; i++)
		{
			_cards[i].PutDown();

			cards[nextFreePosition--] = _cards[i];
		}

		position = nextFreePosition + 1;

		return true;
	}
	public bool AddRange(List<GameCard> _cards)
	{
		int _length = _cards.Count;

		if(nextFreePosition - _length < 0)
		{
			return false;
		}

		for(int i = 0; i < _length; i++)
		{
			_cards[i].PutDown();

			cards[nextFreePosition--] = _cards[i];
		}

		position = nextFreePosition + 1;

		return true;
	}
	public bool Add(GameCard _card)
	{
		if(nextFreePosition < 0)
		{
			return false;
		}

		_card.PutDown();

		cards[nextFreePosition--] = _card;

		position = nextFreePosition + 1;

		return true;
	}
	/// <returns>
	/// Boolean represents if not at end of card deck (true means more cards to go, false means at end)
	/// </returns>
	public bool TryNextFree(out GameCard _card)
	{
		_card = Current();

		while(_card.InUse)
		{
			position--;
			_card = Current();
		}

		_card.PickUp();

		position--;
		return position > 0;
	}
	public void Clear()
	{
		position = cards.Length - 1;
		Array.Clear(cards);
    }
	private GameCard Current()
	{
		position = position < 0 ? 0 : position;
		return cards[position];
	}
}