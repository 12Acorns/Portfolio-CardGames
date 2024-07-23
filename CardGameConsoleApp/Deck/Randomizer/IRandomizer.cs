using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck.Randomizer;

internal interface IRandomizer
{
	public void Randomize(Span<Card.GameCard> _cards); 
}
