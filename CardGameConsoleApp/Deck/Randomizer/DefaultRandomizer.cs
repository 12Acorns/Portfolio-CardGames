using System.Security.Cryptography;

namespace CardGameConsoleApp.Deck.Randomizer;
internal sealed class DefaultRandomizer : IRandomizer
{
	public void Randomize(Span<Card.GameCard> _cards)
	{
		RandomNumberGenerator.Shuffle(_cards);
	}
}
