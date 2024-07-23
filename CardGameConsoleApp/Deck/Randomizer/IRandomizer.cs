namespace CardGameConsoleApp.Deck.Randomizer;

internal interface IRandomizer
{
	public static readonly IRandomizer Default = new DefaultRandomizer();
	public static readonly IRandomizer None = new NoRandomizer();

	public void Randomize(Span<Card.GameCard> _cards); 
}
