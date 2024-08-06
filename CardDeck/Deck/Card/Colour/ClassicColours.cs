namespace Deck.Deck.Card.Colour;

internal readonly struct ClassicColours : IColourSet
{
	public ClassicColours() { }

	public readonly RGBColour Red = RGBColour.Red;
	public readonly RGBColour Green = RGBColour.Green;
	public readonly RGBColour Blue = RGBColour.Blue;
	public readonly RGBColour Yellow = RGBColour.Yellow;
	public readonly RGBColour None = RGBColour.None;

	public string SetName { get; } = "Classic Colours";

	public IEnumerable<RGBColour> GetColours()
	{
		yield return Red;
		yield return Green;
		yield return Blue;
		yield return Yellow;
		yield return None;
	}
}
