using Deck.Deck.Card;

namespace GameOne;

// Do not like this approach
internal static class Globals
{
	static Globals()
	{
		// SubTypes
		// Numeric
		ZeroSubType = new("Z");
		OneSubType = new("O");
		TwoSubType = new("T");
		ThreeSubType = new("Th");
		FourSubType = new("F");
		FiveSubType = new("Fi");
		SixSubType = new("S");
		SevenSubType = new("Se");
		EightSubType = new("E");
		NineSubType = new("N");

		// Special
		PlusTwoSubType = new("P");
		ReverseSubType = new("R");
		SkipSubType = new("B");
		WildSubType = new("W");
		WildPlusFourSubType = new("Wp");

		// Types
		NumericType = new("Nu", (CardSubType[])
			[ZeroSubType, OneSubType, TwoSubType, ThreeSubType,
			 FourSubType, FiveSubType, SixSubType, SevenSubType,
			 EightSubType, NineSubType]);
		SpecialType = new("Sp", (CardSubType[])
			[PlusTwoSubType, SkipSubType, ReverseSubType, WildSubType, WildPlusFourSubType]);
	}

	// Types
	public static readonly CardType NumericType;
	public static readonly CardType SpecialType;

	// SubTypes
	// Numeric
	public static readonly CardSubType ZeroSubType;
	public static readonly CardSubType OneSubType;
	public static readonly CardSubType TwoSubType;
	public static readonly CardSubType ThreeSubType;
	public static readonly CardSubType FourSubType;
	public static readonly CardSubType FiveSubType;
	public static readonly CardSubType SixSubType;
	public static readonly CardSubType SevenSubType;
	public static readonly CardSubType EightSubType;
	public static readonly CardSubType NineSubType;
	//Special
	public static readonly CardSubType PlusTwoSubType;
	public static readonly CardSubType ReverseSubType;
	public static readonly CardSubType SkipSubType;
	public static readonly CardSubType WildSubType;
	public static readonly CardSubType WildPlusFourSubType;
}
