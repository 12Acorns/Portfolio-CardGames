using Deck.Deck.Card;

namespace Deck.Deck;

public readonly struct DeckDescription
{
	public DeckDescription(IEnumerable<CardDescription> _descriptions)
	{
		foreach(var _option in _descriptions)
		{
			TotalCards += _option.TotalCount;
		}

		CardDescriptions = _descriptions;
		DescriptionsLength = CardDescriptions.Count();
	}

	public int DescriptionsLength { get; }
	public int TotalCards { get; }
	public IEnumerable<CardDescription> CardDescriptions { get; }
}
