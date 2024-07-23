using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal readonly struct GeneralDeckOptions
{
	public static GeneralDeckOptions SpecialDefault { get; } = new(
	[
		new(CardColour.Yellow, 
		[
			new(CardType.PlusTwo, CardColour.Yellow),
			new(CardType.PlusTwo, CardColour.Yellow),
			new(CardType.Reverse, CardColour.Yellow),
			new(CardType.Reverse, CardColour.Yellow),
			new(CardType.Block, CardColour.Yellow),
			new(CardType.Block, CardColour.Yellow)
		]),
		new(CardColour.Blue, 
		[
			new(CardType.PlusTwo, CardColour.Blue),
			new(CardType.PlusTwo, CardColour.Blue),
			new(CardType.Reverse, CardColour.Blue),
			new(CardType.Reverse, CardColour.Blue),
			new(CardType.Block, CardColour.Blue),
			new(CardType.Block, CardColour.Blue)
		]),
		new(CardColour.Red, 
		[
			new(CardType.PlusTwo, CardColour.Red),
			new(CardType.PlusTwo, CardColour.Red),
			new(CardType.Reverse, CardColour.Red),
			new(CardType.Reverse, CardColour.Red),
			new(CardType.Block, CardColour.Red),
			new(CardType.Block, CardColour.Red)
		]),
		new(CardColour.Green, 
		[
			new(CardType.PlusTwo, CardColour.Green),
			new(CardType.PlusTwo, CardColour.Green),
			new(CardType.Reverse, CardColour.Green),
			new(CardType.Reverse, CardColour.Green),
			new(CardType.Block, CardColour.Green),
			new(CardType.Block, CardColour.Green)
		]),
		new(CardColour.None,
		[
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None)
		])
	]);
	public static GeneralDeckOptions NumericDefault { get; } = new(
	[
		new CardDataArray(CardColour.Yellow, Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Yellow), 19)),
		new CardDataArray(CardColour.Blue, Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Blue), 19)),
		new CardDataArray(CardColour.Red, Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Red), 19)),
		new CardDataArray(CardColour.Green, Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Green), 19))
	]);

	public GeneralDeckOptions(IEnumerable<CardDataArray> _options)
	{
		foreach(var _option in _options)
		{
			TotalCards += _option.TotalCards;
		}

		Options = _options;
		OptionsLength = Options.Count();
	}

	public int OptionsLength { get; }
	public int TotalCards { get; }
	public IEnumerable<CardDataArray> Options { get; }
}
