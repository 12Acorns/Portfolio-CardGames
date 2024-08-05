using System.Runtime.CompilerServices;
using Deck.Deck.Card.Colour;
using Deck.Deck.Card;

namespace Deck.Deck;

public sealed class CardDescriptionBuilder
{
	private const byte DEFAULTCOUNT = 0;

	public CardDescriptionBuilder(CardType _cardType, IColourSet _set, RGBColour _cardColour)
	{
		set = _set;
		type = _cardType;
		colour = _cardColour;

		cardCountPerSubType = _cardType.TypeMembers
			.ToDictionary(x => x, x => DEFAULTCOUNT);

		methodCallMap = new(_cardType.TypeMembers.Count + 1);
	}

	private readonly CardType type;
	private readonly IColourSet set;
	private readonly RGBColour colour;
	private readonly HashSet<CardSubType> methodCallMap;
	private readonly Dictionary<CardSubType, byte> cardCountPerSubType;

	public CardDescriptionBuilder WithType(CardSubType _type, byte _count)
	{
		ThrowIfNotTypeOrUsed(_type);
		cardCountPerSubType[_type] = _count;
		return this;
	}

	public CardDescription Build()
	{
		return new CardDescription(type, colour, set, cardCountPerSubType);
	}

	#region ThrowIf X
	private void ThrowIfNotTypeOrUsed(CardSubType _type)
	{
		ThrowIfAlreadyUsed(_type);
		ThrowIfNotType(_type);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ThrowIfNotType(CardSubType _type)
	{
		if(!type.TypeMembers.Contains(_type))
		{
			throw new InvalidOperationException(
				$"Invalid operation. Cannot use CardSubType '{_type.SubTypeName}' if " +
				$"current CardType is {type.TypeName}");
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ThrowIfAlreadyUsed(CardSubType _type)
	{
		if(!methodCallMap.Add(_type))
		{
			throw new Exception($"Cannot use '{_type.SubTypeName}' again.");
		}
	}
	#endregion
}