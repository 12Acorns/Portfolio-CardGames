namespace Deck.Deck;

public readonly record struct DeckOptions
{
	public static readonly DeckOptions Default = new();

	/// <summary>
	/// Creates a default instance
	/// </summary>
	public DeckOptions()
	{
		SpecialDeckOptions = DeckFactory.GetDefaultSpecialDescription();
		NumericDeckOptions = DeckFactory.GetDefaultNumericDescription();

		TotalCards = SpecialDeckOptions.TotalCards + NumericDeckOptions.TotalCards;

		HasSpecialCards = true;
		HasNumericCards = true;
	}

	public DeckOptions(
		DeckDescription _specialDeckOptions,
		DeckDescription _numericDeckOptions)
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
	public DeckDescription SpecialDeckOptions { get; }
	public DeckDescription NumericDeckOptions { get; }
}