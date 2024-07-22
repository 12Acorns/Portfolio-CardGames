namespace CardGameConsoleApp.Deck.Card;

internal readonly struct NumericCard : ICard
{
	public NumericCard(CardData _data, byte _value)
	{
		Data = _data;
		Value = _value;
	}

	public CardData Data { get; }
	public int Value { get; }
}
