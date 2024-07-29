namespace Deck.Deck.Card.Colour;

internal readonly struct ClassicColours : IColourSet
{
	public ClassicColours() { }

	public readonly RGBColour Red = new("Red", 255, 0, 0);
	public readonly RGBColour Green = new("Green", 0, 255, 0);
	public readonly RGBColour Blue = new("Blue", 0, 0, 255);
	public readonly RGBColour Yellow = new("Yellow", 255, 255, 0);
	public readonly RGBColour Black = new("Black", 0, 0, 0);

	public string SetName { get; } = "Default Colours";

	public IEnumerable<RGBColour> GetColours()
	{
		yield return Red;
		yield return Green;
		yield return Blue;
		yield return Yellow;
		yield return Black;
	}
}
