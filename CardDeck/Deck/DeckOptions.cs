namespace Deck.Deck;

public readonly record struct DeckOptions
{
	public DeckOptions(
		CardGroupDescription[] _deckOptions)
	{
		GroupOptions = _deckOptions;
		TotalCards = _deckOptions.Sum(x => x.TotalCards);

		HasCardTypeMap = _deckOptions
			.Select(x => x.GroupName)
			.ToHashSet();

		if(_deckOptions.Length == 0)
		{
			throw new Exception("Cannot create a deck with no card types");
		}
		if(TotalCards == 0)
		{
			throw new Exception("Cannot create a empty deck");
		}
	}

	public int TotalCards { get; }
	public CardGroupDescription[] GroupOptions { get; }

	private readonly HashSet<string> HasCardTypeMap;

	public readonly bool HasCardOfType(CardGroupDescription _descriptor) =>
		HasCardOfType(_descriptor.GroupName);
	public readonly bool HasCardOfType(string _name) =>
		HasCardTypeMap.Contains(_name);
}