using System.Runtime.InteropServices;

namespace CardGameConsoleApp.Deck.Card;

internal readonly struct SpecialCard : ICard
{
	public static readonly int Size = Marshal.SizeOf<SpecialCard>();

	public SpecialCard(CardData _data)
	{
		Data = _data;
	}

	public CardData Data { get; }
}