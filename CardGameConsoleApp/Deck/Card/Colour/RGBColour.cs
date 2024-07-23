namespace CardGameConsoleApp.Deck.Card.Colour;

internal readonly struct RGBColour : IColourSet
{
	public RGBColour(byte _r, byte _g, byte _b)
	{
		R = _r;
		G = _g;
		B = _b;
	}

	public byte R { get; }
	public byte G { get; }
	public byte B { get; }

	public IEnumerable<RGBColour> GetColours()
	{
		return [this];
	}
}
