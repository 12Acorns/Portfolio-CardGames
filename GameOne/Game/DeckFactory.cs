using Deck.Deck.Card.Colour;
using Deck.Deck.Card;
using GameOne;

namespace Deck.Deck;

public static class DeckFactory
{
	public static CardGroupDescription GetDefaultSpecialDescription()
	{
		var _descriptions = new CardDescription[5];

		ReadOnlySpan<RGBColour> _colours = IColourSet.Default.GetColours().ToArray();

		for(int i = 0; i < _colours.Length - 1; i++)
		{
			var _cardDescriptionBuilder =
				new CardDescriptionBuilder(Globals.SpecialType, IColourSet.Default, _colours[i]);
			_descriptions[i] = _cardDescriptionBuilder
				.WithType(Globals.PlusTwoSubType, 2).WithType(Globals.ReverseSubType, 2)
				.WithType(Globals.SkipSubType, 2).Build();
		}

		var _wildCardsBuilder =
			new CardDescriptionBuilder(Globals.SpecialType, IColourSet.Default, _colours[^1]);
		_descriptions[4] = _wildCardsBuilder
			.WithType(Globals.WildSubType, 4).WithType(Globals.WildPlusFourSubType, 4)
			.Build();

		return new CardGroupDescription("Special", _descriptions);
	}
	public static CardGroupDescription GetDefaultNumericDescription()
	{
		ReadOnlySpan<RGBColour> _colours = IColourSet.Default.GetColours().ToArray();

		var _descriptions = new CardDescription[_colours.Length - 1];

		for(int i = 0; i < _descriptions.Length; i++)
		{
			var _cardDescriptionBuilder =
				new CardDescriptionBuilder(Globals.NumericType, IColourSet.Default, _colours[i]);
			_cardDescriptionBuilder
				.WithType(Globals.ZeroSubType, 1);

			foreach(var _type in Globals.NumericType.TypeMembers)
			{
				if(_type == Globals.ZeroSubType)
				{
					continue;
				}

				_cardDescriptionBuilder.WithType(_type, 2);
			}

			_descriptions[i] = _cardDescriptionBuilder.Build();
		}

		return new CardGroupDescription("Numeric", _descriptions);
	}
}
