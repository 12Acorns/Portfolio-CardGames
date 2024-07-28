using Deck.Extensions;

namespace Deck.Deck.Card;

public readonly struct CardData
{
	public CardData(CardSubType _subType, Action _behaviour)
	{
		SubType = _subType;
		Score = SubType.MapToScore();
		Behaviour = _behaviour;
	}

	/// <summary>
	/// Used to also represent value to player, handle special type values by showing none
	/// </summary>
	public byte Score { get; }
	public CardSubType SubType { get; }
	public Action Behaviour { get; }
}
