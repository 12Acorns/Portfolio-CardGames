using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck;

internal readonly struct NumericDeckOptions
{
	public static NumericDeckOptions Default { get; } =
		new(Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Yellow), 18).ToArray(),
			Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Yellow), 18).ToArray(),
			Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Yellow), 18).ToArray(),
			Enumerable.Repeat(new CardData(CardType.Numeric, CardColour.Yellow), 18).ToArray());

	public NumericDeckOptions(
		CardData[] _yellowCards, CardData[] _blueCards,
		CardData[] _redCards, CardData[] _greenCards)
	{
		YellowCards = _yellowCards;
		BlueCards = _blueCards;
		RedCards = _redCards;
		GreenCards = _greenCards;
		TotalCards = _yellowCards.Length + _blueCards.Length + _redCards.Length + _greenCards.Length;
	}

	public int TotalCards { get; }
	public CardData[] YellowCards { get; }
	public CardData[] BlueCards { get; }
	public CardData[] RedCards { get; }
	public CardData[] GreenCards { get; }
}