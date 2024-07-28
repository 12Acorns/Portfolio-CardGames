using Deck.Deck.Card.Colour;

namespace Deck.Deck.Card;

internal readonly struct CardDataArray
{
	public CardDataArray(IColourSet _colour, IEnumerable<CardDescription> _cards)
	{
		Colour = _colour;
		if(!_cards.All(x => x.Colour == _colour))
		{
			throw new ArrayTypeMismatchException("Mismatch of specified colour of whole array and actual colour");
		}
		Cards = _cards;
		TotalCards = Cards.Count();
	}
	public CardDataArray(IColourSet _colour, CardDescription[] _cards)
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
	public IColourSet Colour { get; }
	public IEnumerable<CardDescription> Cards { get; }
}
