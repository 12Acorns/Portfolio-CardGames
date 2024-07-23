namespace CardGameConsoleApp.Deck.Card;

internal readonly struct CardDataArray
{
	public CardDataArray(CardColour _colour, IEnumerable<CardData> _cards)
	{
		Colour = _colour;
		if(!Enumerable.All(_cards, x => x.Colour == _colour))
		{
			throw new ArrayTypeMismatchException("Mismatch of specified colour of whole array and actual colour");
		}
		Cards = _cards;
		TotalCards = Cards.Count();
	}
	public CardDataArray(CardColour _colour, CardData[] _cards)
	{
		Colour = _colour;

		if(!Array.TrueForAll(_cards, x => x.Colour == _colour))
		{
			throw new ArrayTypeMismatchException("Mismatch of specified colour of whole array and actual colour");
		}
		Cards = _cards;
		TotalCards = Cards.Count();
	}

	public int TotalCards { get; }
	public CardColour Colour { get; }
	public IEnumerable<CardData> Cards { get; }
}
