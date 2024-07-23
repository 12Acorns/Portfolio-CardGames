using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal readonly struct DeckDescription
{
	public static DeckDescription SpecialDefault { get; } = new(
	[
		// 6 special per colour
		new(CardType.Special, CardColour.Yellow, ),
		new(CardType.Special, CardColour.Blue, ),
		new(CardType.Special, CardColour.Red, ),
		new(CardType.Special, CardColour.Green, ),

		// 8 Wild cards
		new(CardType.Special, CardColour.None, ),
	]);
	public static DeckDescription NumericDefault { get; } = new(
	[
		// 19 per colour
		new CardDescription(CardType.Numeric, CardColour.Yellow, ),
		new CardDescription(CardType.Numeric, CardColour.Blue, ),
		new CardDescription(CardType.Numeric, CardColour.Red, ),
		new CardDescription(CardType.Numeric, CardColour.Green, )
	]);

	public DeckDescription(IEnumerable<CardDescription> _descriptions)
	{
		foreach(var _option in _descriptions)
		{
			TotalCards += _option.TotalCount;
		}

		CardDescriptions = _descriptions;
		DescriptionsLength = CardDescriptions.Count();
	}

	public int DescriptionsLength { get; }
	public int TotalCards { get; }
	public IEnumerable<CardDescription> CardDescriptions { get; }
}
