namespace Deck.Deck.Card;

public readonly struct CardData
{
	public CardData(CardSubType _subType, Func<CardSubType, byte> _scoreMapping)
	{
		SubType = _subType;
		Score = _scoreMapping(_subType);
	}

	/// <summary>
	/// Used to also represent value to player, handle special type values by showing none
	/// </summary>
	public byte Score { get; }
	public CardSubType SubType { get; }
}
