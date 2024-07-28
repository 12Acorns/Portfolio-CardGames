using Deck.Deck.Card;

namespace Deck.Deck.Randomizer;

public sealed class KnuthFisherRandomizer : IRandomizer
{
	public void Randomize(Span<GameCard> _cards)
	{
		var _random = new Random();

		for(int i = _cards.Length - 1; i > 0; --i)
		{
			int _position = _random.Next(i + 1);
			(_cards[_position], _cards[i]) = (_cards[i], _cards[_position]);
		}
	}
}
