using CardGameConsoleApp.Deck.Card;

namespace CardGameConsoleApp.Deck.Randomizer;
internal class NoRandomizer : IRandomizer
{
	public void Randomize(Span<GameCard> _cards) { }
}
