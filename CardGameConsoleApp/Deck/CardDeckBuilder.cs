using CardGameConsoleApp.Extensions;
using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal sealed class CardDeckBuilder
{
	private DeckOptions options = DeckOptions.Default;

	public CardDeckBuilder WithCustomDeckOptions(DeckOptions _options)
	{
		options = _options;
		return this;
	}
	public CardDeck Build()
	{
		var _cards = CreateCards();

		return new CardDeck();
	}

	private ReadOnlySpan<ICard> CreateCards()
	{
		var _cards = new ICard[options.TotalCards].AsSpan();

		var _specialCardOptions = options.SpecialDeckOptions;
		var _numericCardOptions = options.NumericDeckOptions;

		CreateSpecialCards(0, _specialCardOptions, _cards, out int _offset);
		CreateNumericCards(_offset, _numericCardOptions, _cards, out _);

		return _cards;
	}

	private static void CreateNumericCards(int _initialIndex, GeneralDeckOptions _numericDeckOptions,
		Span<ICard> _source, out int _endIndex)
	{
		CreateCardOfType(CardType.Numeric, _initialIndex, _numericDeckOptions, _source, out _endIndex);
	}
	private static void CreateSpecialCards(int _initialIndex, GeneralDeckOptions _specialDeckOptions,
		Span<ICard> _source, out int _endIndex)
	{
		CreateCardOfType(CardType.PlusTwo, _initialIndex, _specialDeckOptions, _source, out int _index);
		CreateCardOfType(CardType.Reverse, _index, _specialDeckOptions, _source, out _index);
		CreateCardOfType(CardType.Block, _index, _specialDeckOptions, _source, out _index);
		CreateCardOfType(CardType.WildColour, _index, _specialDeckOptions, _source, out _index);
		CreateCardOfType(CardType.WildPlusFour, _index, _specialDeckOptions, _source, out _endIndex);
	}
	private static void CreateCardOfType(CardType _type, int _initialIndex, GeneralDeckOptions _deckOptions,
		Span<ICard> _source, out int _endIndex)
	{
		var _cardColourAndCount = BindColourAndIndex(_type, _deckOptions);
		CreateCardType(_source, _initialIndex, _type, out _endIndex, _cardColourAndCount);
	}
	private static (CardColour _colour, int _count)[] 
		BindColourAndIndex(CardType _type, GeneralDeckOptions _deckOptions)
	{
		var _cardColourAndCounts = new (CardColour _colour, int _count)[_deckOptions.OptionsLength];

		int i = 0;
		foreach(var _card in _deckOptions.Options)
		{
			_cardColourAndCounts[i] = (_card.Colour, _card.Cards.SumOfCardsOfType(_type));
			i++;
		}

		return _cardColourAndCounts;
	}
	private static void CreateCardType(Span<ICard> _source, int _offset, CardType _type, 
		out int _endIndex, (CardColour _colour, int _count)[] _colourLengths)
	{
		_endIndex = _offset;

		for(int i = 0; i < _colourLengths.Length; i++)
		{
			int _length = _colourLengths[i]._count;
			for(int j = 0; j < _length; j++, _endIndex++)
			{
				_source[_endIndex] = 
					new SpecialCard(new CardData(_type, _colourLengths[i]._colour));
			}
		}
	}
}