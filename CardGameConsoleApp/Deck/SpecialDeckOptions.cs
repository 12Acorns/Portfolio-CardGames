using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal readonly struct SpecialDeckOptions
{
	public static SpecialDeckOptions Default { get; } =
		new(
		[
			new(CardType.PlusTwo, CardColour.Yellow),
			new(CardType.Reverse, CardColour.Yellow),
			new(CardType.Block, CardColour.Yellow),
		],
		[
			new(CardType.PlusTwo, CardColour.Blue),
			new(CardType.Reverse, CardColour.Blue),
			new(CardType.Block, CardColour.Blue),
		],
		[ 
			new(CardType.PlusTwo, CardColour.Red),
			new(CardType.Reverse, CardColour.Red),
			new(CardType.Block, CardColour.Red),
		],
		[
			new(CardType.PlusTwo, CardColour.Green),
			new(CardType.Reverse, CardColour.Green),
			new(CardType.Block, CardColour.Green),
		],
		[
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildColour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None),
			new(CardType.WildPlusFour, CardColour.None)
		]);

	public SpecialDeckOptions(
		CardData[] _yellowCards, CardData[] _blueCards, 
		CardData[] _redCards, CardData[] _greenCards,
		CardData[] _wildCards)
	{
		YellowCards = _yellowCards;
		BlueCards = _blueCards;
		RedCards = _redCards;
		GreenCards = _greenCards;
		WildCards = _wildCards;
		TotalCards = _yellowCards.Length + _blueCards.Length + _redCards.Length + _greenCards.Length + _wildCards.Length;
	}

	public int TotalCards { get; }
	public CardData[] YellowCards { get; }
	public CardData[] BlueCards { get; }
	public CardData[] RedCards { get; }
	public CardData[] GreenCards { get; }
	public CardData[] WildCards { get; }
}
