using Deck.Deck.Card.Colour;
using Deck.Deck.Card;

namespace Deck.Deck;

public static class DeckFactory
{
	public static DeckDescription GetDefaultSpecialDescription()
	{
		var _descriptions = new CardDescription[5];

		ReadOnlySpan<RGBColour> _colours = IColourSet.Default.GetColours().ToArray();

		for(int i = 0; i < _colours.Length - 1; i++)
		{
			var _cardDescriptionBuilder =
				new CardDescriptionBuilder(CardType.Special, IColourSet.Default, _colours[i]);
			_descriptions[i] = _cardDescriptionBuilder
				.WithPlusTwo(2).WithReverse(2)
				.WithSkip(2).Build();
		}

		var _wildCardsBuilder =
			new CardDescriptionBuilder(CardType.Special, IColourSet.Default, _colours[^1]);
		_descriptions[4] = _wildCardsBuilder
			.WithWild(4).WithWildPlusFour(4)
			.Build();

		return new DeckDescription(_descriptions);
	}
	public static DeckDescription GetDefaultNumericDescription()
	{
		ReadOnlySpan<RGBColour> _colours = IColourSet.Default.GetColours().ToArray();

		var _descriptions = new CardDescription[_colours.Length - 1];

		for(int i = 0; i < _descriptions.Length; i++)
		{
			var _cardDescriptionBuilder =
				new CardDescriptionBuilder(CardType.Numeric, IColourSet.Default, _colours[i]);
			_descriptions[i] = _cardDescriptionBuilder
				.WithZero(1).WithOne(2)
				.WithTwo(2).WithThree(2)
				.WithFour(2).WithFive(2)
				.WithSix(2).WithSeven(2)
				.WithEight(2).WithNine(2)
				.Build();
		}

		return new DeckDescription(_descriptions);
	}
}
