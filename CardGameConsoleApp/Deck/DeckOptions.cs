namespace CardGameConsoleApp.Deck;

internal readonly record struct DeckOptions
{
	public static readonly DeckOptions Default = new();

	public DeckOptions(
		SpecialDeckOptions _specialDeckOptions,
		NumericDeckOptions _numericDeckOptions)
	{
		SpecialDeckOptions = _specialDeckOptions;
		NumericDeckOptions = _numericDeckOptions;
		MaximumCards = _specialDeckOptions.TotalCards;

		HasSpecialCards = _specialDeckOptions.TotalCards != 0;
		HasNumericCards = _numericDeckOptions.TotalCards != 0;

		if(!HasSpecialCards && HasNumericCards)
		{
			throw new Exception("Cannot create a deck with no card types");
		}
		if(MaximumCards == 0)
		{
			throw new Exception("Cannot create a empty deck");
		}
	}

	public bool HasSpecialCards { get; }
	public bool HasNumericCards { get; }

	public int MaximumCards { get; }
	public SpecialDeckOptions SpecialDeckOptions { get; }
	public NumericDeckOptions NumericDeckOptions { get; }
}