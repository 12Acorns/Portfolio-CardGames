using Deck.Deck.Card;

namespace Deck.Deck.Randomizer;

public interface IRandomizer
{
	public void Randomize(Span<GameCard> _cards);
}
