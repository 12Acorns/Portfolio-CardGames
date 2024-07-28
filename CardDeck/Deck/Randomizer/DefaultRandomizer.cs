using System.Security.Cryptography;

namespace Deck.Deck.Randomizer;

public sealed class DefaultRandomizer : IRandomizer
{
	public void Randomize(Span<Card.GameCard> _cards)
	{
		RandomNumberGenerator.Shuffle(_cards);
	}
}
