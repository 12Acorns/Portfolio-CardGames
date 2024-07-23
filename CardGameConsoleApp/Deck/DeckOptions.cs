namespace CardGameConsoleApp.Deck;

internal readonly record struct DeckOptions
{
	public static readonly DeckOptions Default = new();

	public DeckOptions()
	{
		SpecialDeckOptions = GeneralDeckOptions.SpecialDefault;
		NumericDeckOptions = GeneralDeckOptions.NumericDefault;

		TotalCards = SpecialDeckOptions.TotalCards + NumericDeckOptions.TotalCards;

		HasSpecialCards = true;
		HasNumericCards = true;
	}

	public DeckOptions(
		GeneralDeckOptions _specialDeckOptions,
		GeneralDeckOptions _numericDeckOptions)
	{
		SpecialDeckOptions = _specialDeckOptions;
		NumericDeckOptions = _numericDeckOptions;
		TotalCards = _specialDeckOptions.TotalCards;

		HasSpecialCards = _specialDeckOptions.TotalCards != 0;
		HasNumericCards = _numericDeckOptions.TotalCards != 0;

		if(!HasSpecialCards && !HasNumericCards)
		{
			throw new Exception("Cannot create a deck with no card types");
		}
		if(TotalCards == 0)
		{
			throw new Exception("Cannot create a empty deck");
		}
	}

	public bool HasSpecialCards { get; }
	public bool HasNumericCards { get; }

	public int TotalCards { get; }
	public GeneralDeckOptions SpecialDeckOptions { get; }
	public GeneralDeckOptions NumericDeckOptions { get; }
}