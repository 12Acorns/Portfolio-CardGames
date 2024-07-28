namespace Deck.Deck.Card.Colour;

internal readonly struct ClassicColours : IColourSet
{
	public ClassicColours() { }

	public readonly RGBColour Red = new(255, 0, 0);
	public readonly RGBColour Green = new(0, 255, 0);
	public readonly RGBColour Blue = new(0, 0, 255);
	public readonly RGBColour Yellow = new(255, 255, 0);

	public IEnumerable<RGBColour> GetColours()
	{
		yield return Red;
		yield return Green;
		yield return Blue;
		yield return Yellow;
	}
}
