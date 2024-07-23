using CardGameConsoleApp.Deck.Card;
using System.Security.Cryptography;

namespace CardGameConsoleApp.Deck.Randomizer;
internal sealed class DefaultRandomizer : IRandomizer
{
	private static readonly DefaultRandomizer instance = new();
	public static DefaultRandomizer Instance
	{
		get
		{
			return instance;
		}
	}

	public void Randomize(Span<Card.GameCard> _cards)
	{
		RandomNumberGenerator.Shuffle(_cards);
	}
}
