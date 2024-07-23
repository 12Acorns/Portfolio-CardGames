using CardGameConsoleApp.Deck.Card;
using System.Runtime.CompilerServices;

namespace CardGameConsoleApp.Extensions;

internal static class CardExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SumOfCardsOfType(this IEnumerable<CardData> _card, CardType _type) =>
		_card.Count(x => x.Type == _type);
}
