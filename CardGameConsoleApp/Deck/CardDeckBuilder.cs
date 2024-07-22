namespace CardGameConsoleApp.Deck;

internal sealed class CardDeckBuilder
{
	public DeckOptions options = DeckOptions.Default;

	public CardDeckBuilder WithCustomDeckOptions(DeckOptions _options)
	{
		throw new NotImplementedException();
		return this;
	}
	public CardDeck Build()
	{
		throw new NotImplementedException();
	}
}