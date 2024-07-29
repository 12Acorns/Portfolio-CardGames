using Deck.Deck;
using Deck.Deck.Card;
using System.Runtime.CompilerServices;

namespace Deck.Extensions;

public static class CardExtensions
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
	public static bool CanPlay(this GameCard _card, GameCard _other, bool _strictPlay = true)
	{
		var _discardDescription = _other.Description;
		var _desiredDescription = _card.Description;
		var _discardData = _other.Data;
		var _desiredData = _card.Data;

		if(_card.InUse && _strictPlay)
		{
			return false;
		}

		// Same Colour
		return _discardDescription.Colour == _desiredDescription.Colour
			// Same Sub Type
			|| _discardData.SubType == _desiredData.SubType
			// Wild Card
			|| _card.Data.SubType is CardSubType.Wild or CardSubType.WildPlusFour;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool CanPlay(this GameCard _card, CardDeck _deck, bool _strictPlay = true)
	{
		return _card.CanPlay(_deck.Peek(), _strictPlay);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNumercic(this CardSubType _subType)
	{
		return _subType is CardSubType.Zero or CardSubType.One
						or CardSubType.Two or CardSubType.Three
						or CardSubType.Four or CardSubType.Five
						or CardSubType.Six or CardSubType.Seven
						or CardSubType.Eight or CardSubType.Nine;
	}
}
