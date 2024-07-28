using Deck.Deck.Card.Colour;

namespace Deck.Deck.Card.Colour;

public interface IColourSet
{
	public static readonly IColourSet Default = new ClassicColours();

	public IEnumerable<RGBColour> GetColours();
}
