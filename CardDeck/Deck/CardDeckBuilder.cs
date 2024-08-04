using Deck.Deck.Randomizer;
using Deck.Deck.Card;

namespace Deck.Deck;

public sealed class CardDeckBuilder
{
	public CardDeckBuilder(DeckOptions _options, Func<CardSubType, byte> _scoreMapping)
	{
		scoreMapping = _scoreMapping;
		options = _options;
	}

	private readonly Func<CardSubType, byte> scoreMapping;
	private readonly DeckOptions options;

	private IRandomizer shuffleOptions = RandomizerFactory.Get(RandomizerType.DefualtRandom);

	public CardDeckBuilder WithCustomShuffleOptions(IRandomizer _randomizer)
	{
		shuffleOptions = _randomizer;
		return this;
	}
	public CardDeck Build()
	{
		var _cards = CreateCards();

		var _deck = new CardDeck(_cards, shuffleOptions, _cards.Length);
		_deck.Shuffle(shuffleOptions);

		return _deck;
	}
	private Span<GameCard> CreateCards()
	{
		var _cards = new GameCard[options.TotalCards].AsSpan();

		int _index = 0;
		foreach(var _groupDescriptor in options.GroupOptions)
		{
			CreateCardsOfType(_cards, _index, _groupDescriptor, out _index);
		}

		return _cards;
	}
	private void CreateCardsOfType(Span<GameCard> _source, int _initialIndex,
		CardGroupDescription _deckDescription, out int _endIndex)
	{
		_endIndex = _initialIndex;


		foreach(var _cardDescription in _deckDescription.CardDescriptions)
		{
			var _types = _cardDescription.CardCountMapping
				.Where(x => x.Value > 0)
				.Select(x => x.Key);

			foreach(var _type in _types)
			{
				CreateCardType(_source, _endIndex, _cardDescription, _type, out _endIndex);
			}
		}
	}
	private void CreateCardType(Span<GameCard> _source, int _offset, CardDescription _description,
		CardSubType _subType, out int _endIndex)
	{
		_endIndex = _offset;

		int _count = _description.CardCountMapping[_subType];

		for(int j = 0; j < _count; j++, _endIndex++)
		{
			_source[_endIndex] =
				new GameCard(_description, new CardData(_subType, scoreMapping));
		}
	}
}