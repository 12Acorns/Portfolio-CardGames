using CardGameConsoleApp.Deck.Card.Colour;
using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;
internal static class DeckFactory
{
	public static DeckDescription GetDefaultSpecialDescription()
	{
		var _yellowSpecialCardsBuilder =
			new CardDescriptionBuilder(CardType.Special, new RGBColour(255, 0, 255));
		var _yellowSpecialDescription = _yellowSpecialCardsBuilder
			.WithPlusTwo(2).WithReverse(2)
			.WithSkip(2).Build();

		var _blueSpecialCardsBuilder =
			new CardDescriptionBuilder(CardType.Special, new RGBColour(0, 0, 255));
		var _blueSpecialDescription = _blueSpecialCardsBuilder
			.WithPlusTwo(2).WithReverse(2)
			.WithSkip(2).Build();

		var _redSpecialCardsBuilder =
			new CardDescriptionBuilder(CardType.Special, new RGBColour(255, 0, 0));
		var _redSpecialDescription = _redSpecialCardsBuilder
			.WithPlusTwo(2).WithReverse(2)
			.WithSkip(2).Build();

		var _greenSpecialCardsBuilder =
			new CardDescriptionBuilder(CardType.Special, new RGBColour(255, 255, 0));
		var _greenSpecialDescription = _greenSpecialCardsBuilder
			.WithPlusTwo(2).WithReverse(2)
			.WithSkip(2).Build();

		var _wildCardsBuilder =
			new CardDescriptionBuilder(CardType.Special, new RGBColour(0, 0, 0));
		var _wildSpecialDescription = _wildCardsBuilder
			.WithWild(4).WithWildPlusFour(4)
			.Build();

		return new DeckDescription([_yellowSpecialDescription, _blueSpecialDescription, _redSpecialDescription, _greenSpecialDescription, _wildSpecialDescription]);
	}
	public static DeckDescription GetDefaultNumericDescription()
	{
		var _yellowNumericCardsBuilder = 
			new CardDescriptionBuilder(CardType.Numeric, new RGBColour(255, 0, 255));
		var _yellowNumericDescription = _yellowNumericCardsBuilder
			.WithZero(1).WithOne(2)
			.WithThree(2).WithFour(2)
			.WithFive(2).WithSix(2)
			.WithSeven(2).WithEight(2)
			.WithNine(2).Build();

		var _blueNumericCardsBuilder = 
			new CardDescriptionBuilder(CardType.Numeric, new RGBColour(0, 0, 255));
		var _blueNumericDescription = _blueNumericCardsBuilder
			.WithZero(1).WithOne(2)
			.WithThree(2).WithFour(2)
			.WithFive(2).WithSix(2)
			.WithSeven(2).WithEight(2)
			.WithNine(2).Build();

		var _redNumericCardsBuilder = 
			new CardDescriptionBuilder(CardType.Numeric, new RGBColour(255, 0, 0));
		var _redNumericDescription = _redNumericCardsBuilder
			.WithZero(1).WithOne(2)
			.WithThree(2).WithFour(2)
			.WithFive(2).WithSix(2)
			.WithSeven(2).WithEight(2)
			.WithNine(2).Build();

		var _greenNumericCardsBuilder = 
			new CardDescriptionBuilder(CardType.Numeric, new RGBColour(255, 255, 0));
		var _greenNumericDescription = _greenNumericCardsBuilder
			.WithZero(1).WithOne(2)
			.WithThree(2).WithFour(2)
			.WithFive(2).WithSix(2)
			.WithSeven(2).WithEight(2)
			.WithNine(2).Build();

		return new DeckDescription([_yellowNumericDescription, _blueNumericDescription, _redNumericDescription, _greenNumericDescription]);
	}
}
