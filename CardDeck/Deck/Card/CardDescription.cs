using System.Collections.Frozen;
using Deck.Deck.Card.Colour;

namespace Deck.Deck.Card;

public record CardDescription
{
	public CardDescription(CardType _type, IColour _colour, IColourSet _colourSet, Dictionary<CardSubType, byte> _cardCountMapping)
	{
		Type = _type;
		Colour = _colour;
		ColourSet = _colourSet;
		CardCountMapping = _cardCountMapping.ToFrozenDictionary();
		TotalCount = CardCountMapping.Values.Sum(x => x);
	}

	public CardType Type { get; }
	public IColour Colour { get; set; }
	public IColourSet ColourSet { get; }
	public FrozenDictionary<CardSubType, byte> CardCountMapping { get; }
	public int TotalCount { get; }
}
