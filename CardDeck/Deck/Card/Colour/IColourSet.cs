namespace Deck.Deck.Card.Colour;

public interface IColourSet
{
	public static readonly IColourSet Classic = new ClassicColours();

	public string SetName { get; }

	public IEnumerable<RGBColour> GetColours();

	public int GetHashCode()
	{
		return HashCode.Combine(GetColours());
	}
}
