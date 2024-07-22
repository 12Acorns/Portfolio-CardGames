namespace CardGameConsoleApp.Deck.Card;

internal readonly struct SpecialCard : ICard
{
	public SpecialCard(CardData _data)
	{
		Data = _data;
	}

	public CardData Data { get; }
}