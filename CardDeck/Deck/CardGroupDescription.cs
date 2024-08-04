using Deck.Deck.Card;

namespace Deck.Deck;

public readonly struct CardGroupDescription
{
	public CardGroupDescription(string _groupName, IEnumerable<CardDescription> _descriptions)
	{
		GroupName = _groupName;
		CardDescriptions = _descriptions;
		DescriptionsLength = CardDescriptions.Count();
		TotalCards = _descriptions.Sum(x => x.TotalCount);
	}

	public string GroupName { get; }
	public int DescriptionsLength { get; }
	public int TotalCards { get; }
	public IEnumerable<CardDescription> CardDescriptions { get; }
}
