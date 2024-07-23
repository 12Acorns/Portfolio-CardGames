using CardGameConsoleApp.Deck.Card;
using System.Runtime.CompilerServices;

namespace CardGameConsoleApp.Extensions;

internal static class CardExtensions
{
	private static readonly IReadOnlyDictionary<CardSubType, byte> scoreMappingTable = new Dictionary<CardSubType, byte>()
	{
		// Numeric
		{ CardSubType.Zero, 0 },
		{ CardSubType.One, 1 },
		{ CardSubType.Two, 2 },
		{ CardSubType.Three, 3 },
		{ CardSubType.Four, 4 },
		{ CardSubType.Five, 5 },
		{ CardSubType.Six, 6 },
		{ CardSubType.Seven, 7 },
		{ CardSubType.Eight, 8 },
		{ CardSubType.Nine, 9 },

		// Special
		{ CardSubType.PlusTwo, 20 },
		{ CardSubType.Reverse, 20 },
		{ CardSubType.Skip, 20 },

		// Wild (Special)
		{ CardSubType.WildPlusFour, 50 },
		{ CardSubType.Wild, 50 },
	};

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte MapToScore(this CardSubType _type) => scoreMappingTable[_type];
}
