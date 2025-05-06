using System.Runtime.CompilerServices;
using System.Collections.Frozen;
using Deck.Deck.Card;
using Deck.Deck;
using GameOne;

namespace Deck.Extensions;

internal static class CardExtensions
{
	private static readonly FrozenDictionary<CardSubType, byte> scoreMappingTable = new Dictionary<CardSubType, byte>()
	{
		// Numeric
		{ Globals.ZeroSubType, 0 },
		{ Globals.OneSubType, 1 },
		{ Globals.TwoSubType, 2 },
		{ Globals.ThreeSubType, 3 },
		{ Globals.FourSubType, 4 },
		{ Globals.FiveSubType, 5 },
		{ Globals.SixSubType, 6 },
		{ Globals.SevenSubType, 7 },
		{ Globals.EightSubType, 8 },
		{ Globals.NineSubType, 9 },

		// Special
		{ Globals.PlusTwoSubType, 20 },
		{ Globals.SkipSubType, 20 },
		{ Globals.ReverseSubType, 20 },

		// Wild (Special)
		{ Globals.WildPlusFourSubType, 50 },
		{ Globals.WildSubType, 50 },
	}.ToFrozenDictionary();
	private static readonly FrozenDictionary<CardSubType, string> nameMappingTable = new Dictionary<CardSubType, string>()
	{
		// Numeric
		{ Globals.ZeroSubType, "Zero" },
		{ Globals.OneSubType, "One" },
		{ Globals.TwoSubType, "Two" },
		{ Globals.ThreeSubType, "Three" },
		{ Globals.FourSubType, "Four" },
		{ Globals.FiveSubType, "Five" },
		{ Globals.SixSubType, "Six" },
		{ Globals.SevenSubType, "Seven" },
		{ Globals.EightSubType, "Eight" },
		{ Globals.NineSubType, "Nine" },

		// Special
		{ Globals.PlusTwoSubType, "Plus Two" },
		{ Globals.SkipSubType, "Skip" },
		{ Globals.ReverseSubType, "Reverse" },

		// Wild (Special)
		{ Globals.WildPlusFourSubType, "Wild Plus Four" },
		{ Globals.WildSubType, "Wild" },
	}.ToFrozenDictionary();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte MapToScore(this CardSubType _subType)
	{
		if(!scoreMappingTable.TryGetValue(_subType, out var _score))
		{
			throw new ArgumentException($"Provided type does not exist in mapping. CardSubType: {_subType.SubTypeName}");
		}
		return _score;
	}
	public static bool CanPlay(this GameCard _card, GameCard _comparison, bool _strictPlay = true)
	{
		var _discardDescription = _comparison.Description;
		var _desiredDescription = _card.Description;
		var _discardData = _comparison.Data;
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
			|| _card.Data.SubType == Globals.WildSubType 
			|| _card.Data.SubType == Globals.WildPlusFourSubType;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool CanPlay(this GameCard _card, CardDeck _deck, bool _strictPlay = true) =>
		_card.CanPlay(_deck.Peek(), _strictPlay);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNumercic(this CardSubType _subType) => 
			   _subType == Globals.ZeroSubType
			|| _subType == Globals.OneSubType
			|| _subType == Globals.TwoSubType
			|| _subType == Globals.ThreeSubType
			|| _subType == Globals.FourSubType
			|| _subType == Globals.FiveSubType
			|| _subType == Globals.SixSubType
			|| _subType == Globals.SevenSubType
			|| _subType == Globals.EightSubType
			|| _subType == Globals.NineSubType;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsWild(this CardSubType _subType) =>
		_subType == Globals.WildPlusFourSubType ||
		_subType == Globals.WildSubType;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ReadableNameMapping(this CardSubType _subType)
	{
		if(!nameMappingTable.TryGetValue(_subType, out var _name))
		{
			throw new ArgumentException($"Provided type does not exist in mapping. CardSubType: {_subType.SubTypeName}");
		}
		return _name;
	}
}