using CardGameConsoleApp.Deck.Card.Colour;
using CardGameConsoleApp.Deck.Randomizer;
using CardGameConsoleApp.Extensions;
using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal sealed class CardDeckBuilder
{
	private IRandomizer randomizer = DefaultRandomizer.Instance;
	private DeckOptions options = DeckOptions.Default;

	public CardDeckBuilder WithCustomDeckOptions(DeckOptions _options)
	{
		options = _options;
		return this;
	}
	public CardDeckBuilder WithColourSet(IColourSet _set)
	{
		throw new NotImplementedException();
		return this;
	}
	public CardDeckBuilder WithCustomRandomizeOptions(IRandomizer _randomizer)
	{
		randomizer = _randomizer;
		return this;
	}
	public CardDeck Build()
	{
		var _cards = CreateCards();
		RandomizeCards(_cards);

		return new CardDeck(_cards);
	}

	private void RandomizeCards(Span<GameCard> _cards)
	{
		randomizer.Randomize(_cards);
	}
	private Span<GameCard> CreateCards()
	{
		var _cards = new GameCard[options.TotalCards].AsSpan();

		var _specialCardOptions = options.SpecialDeckOptions;
		var _numericCardOptions = options.NumericDeckOptions;

		CreateSpecialCards(0, _specialCardOptions, _cards, out int _offset);
		CreateNumericCards(_offset, _numericCardOptions, _cards, out _);

		return _cards;
	}

	private static void CreateNumericCards(int _initialIndex, DeckDescription _deckOptions,
		Span<GameCard> _source, out int _endIndex)
	{
		CreateCardOfType(_source, _initialIndex, _deckOptions, CardSubType.Zero, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.One, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Two, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Three, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Four, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Five, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Six, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Seven, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Eight, out _endIndex);
		CreateCardOfType(_source, _endIndex, _deckOptions, CardSubType.Nine, out _endIndex);
	}
	private static void CreateSpecialCards(int _initialIndex, DeckDescription _description,
		Span<GameCard> _source, out int _endIndex)
	{
		CreateCardOfType(_source, _initialIndex, _description, CardSubType.PlusTwo, out _endIndex);
		CreateCardOfType(_source, _endIndex, _description, CardSubType.Skip, out _endIndex);
		CreateCardOfType(_source, _endIndex, _description, CardSubType.Reverse, out _endIndex);
		CreateCardOfType(_source, _endIndex, _description, CardSubType.Wild, out _endIndex);
		CreateCardOfType(_source, _endIndex, _description, CardSubType.WildPlusFour, out _endIndex);
	}
	private static void CreateCardOfType(Span<GameCard> _source, int _initialIndex,
		DeckDescription _deckDescription, CardSubType _type, out int _endIndex)
	{
		_endIndex = _initialIndex;
		foreach(var _cardDescription in _deckDescription.CardDescriptions)
		{
			CreateCardType(_source, _endIndex, _cardDescription, _type, out _endIndex);
		}
	}
	private static void CreateCardType(Span<GameCard> _source, int _offset, CardDescription _description, 
		CardSubType _subType, out int _endIndex)
	{
		_endIndex = _offset;

		int _count = _description.CardCountMapping[_subType];

		for(int j = 0; j < _count; j++, _endIndex++)
		{
			_source[_endIndex] =
				new GameCard(_description, new CardData(_subType));
		}
	}
}