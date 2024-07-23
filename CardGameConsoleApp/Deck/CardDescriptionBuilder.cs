using CardGameConsoleApp.Deck.Card.Colour;
using CardGameConsoleApp.Deck.Card;
using System.Runtime.CompilerServices;

namespace CardGameConsoleApp.Deck;

internal sealed class CardDescriptionBuilder
{
	public CardDescriptionBuilder(CardType _cardType, IColourSet _cardColour)
	{
		type = _cardType;
		colour = _cardColour;

		switch(_cardType)
		{
			case CardType.Numeric:
				cardCountPerSubType = new()
				{
					// Numeric
					{ CardSubType.Zero, 0 },
					{ CardSubType.One, 0 },
					{ CardSubType.Two, 0 },
					{ CardSubType.Three, 0 },
					{ CardSubType.Four, 0 },
					{ CardSubType.Five, 0 },
					{ CardSubType.Six, 0 },
					{ CardSubType.Seven, 0 },
					{ CardSubType.Eight, 0 },
					{ CardSubType.Nine, 0 },
				};
				break;
			case CardType.Special:
				cardCountPerSubType = new()
				{
					// Special
					{ CardSubType.PlusTwo, 0 },
					{ CardSubType.Skip, 0 },
					{ CardSubType.Reverse, 0 },
					
					// Wild (Special)
					{ CardSubType.WildPlusFour, 0 },
					{ CardSubType.Wild, 0 },
				};
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(_cardType));
		}
	}

	private readonly CardType type;
	private readonly IColourSet colour;
	private readonly Dictionary<CardSubType, byte> cardCountPerSubType;

	#region Numeric Cards
	public CardDescriptionBuilder WithZero(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Zero] = _count;
		return this;
	}
	public CardDescriptionBuilder WithOne(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.One] = _count;
		return this;
	}
	public CardDescriptionBuilder WithTwo(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Two] = _count;
		return this;
	}
	public CardDescriptionBuilder WithThree(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Three] = _count;
		return this;
	}
	public CardDescriptionBuilder WithFour(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Four] = _count;
		return this;
	}
	public CardDescriptionBuilder WithFive(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Five] = _count;
		return this;
	}
	public CardDescriptionBuilder WithSix(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Six] = _count;
		return this;
	}
	public CardDescriptionBuilder WithSeven(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Seven] = _count;
		return this;
	}
	public CardDescriptionBuilder WithEight(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Eight] = _count;
		return this;
	}
	public CardDescriptionBuilder WithNine(byte _count)
	{
		ThrowIfSpecial();
		cardCountPerSubType[CardSubType.Nine] = _count;
		return this;
	}
	#endregion
	#region Special Cards
	public CardDescriptionBuilder WithPlusTwo(byte _count)
	{
		ThrowIfNumeric();
		cardCountPerSubType[CardSubType.PlusTwo] = _count;
		return this;
	}
	public CardDescriptionBuilder WithReverse(byte _count)
	{
		ThrowIfNumeric();
		cardCountPerSubType[CardSubType.Reverse] = _count;
		return this;
	}
	public CardDescriptionBuilder WithSkip(byte _count)
	{
		ThrowIfNumeric();
		cardCountPerSubType[CardSubType.Skip] = _count;
		return this;
	}
	public CardDescriptionBuilder WithWild(byte _count)
	{
		ThrowIfNumeric();
		cardCountPerSubType[CardSubType.Wild] = _count;
		return this;
	}
	public CardDescriptionBuilder WithWildPlusFour(byte _count)
	{
		ThrowIfNumeric();
		cardCountPerSubType[CardSubType.WildPlusFour] = _count;
		return this;
	}
	#endregion

	public CardDescription Build()
	{
		return new CardDescription(type, CardColour.None, cardCountPerSubType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ThrowIfNumeric()
	{
		if(type != CardType.Special)
		{
			throw new InvalidOperationException($"Invalid operation. Cannot use Numeric Card methods if " +
				$"current state is {CardType.Special}");
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ThrowIfSpecial()
	{
		if(type != CardType.Numeric)
		{
			throw new InvalidOperationException($"Invalid operation. Cannot use Special Card methods if " +
				$"current state is {CardType.Numeric}");
		}
	}
}