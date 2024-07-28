using Deck.Deck.Card.Colour;

namespace Deck.Deck.Card;

public record CardDescription
{
	public CardDescription(CardType _type, IColourSet _colour, Dictionary<CardSubType, byte> _cardCountMapping)
	{
		Type = _type;
		Colour = _colour;
		CardCountMapping = _cardCountMapping.AsReadOnly();
		TotalCount = CardCountMapping.Values.Sum(x => x);
	}

	public CardType Type { get; }
	public IColourSet Colour { get; }
	public IReadOnlyDictionary<CardSubType, byte> CardCountMapping { get; }
	public int TotalCount { get; }
}
