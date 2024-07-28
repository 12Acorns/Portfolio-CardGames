using Deck.Deck.Card;

namespace Deck.Deck.Randomizer;

public class NoRandomizer : IRandomizer
{
	public void Randomize(Span<GameCard> _cards) { }
}
