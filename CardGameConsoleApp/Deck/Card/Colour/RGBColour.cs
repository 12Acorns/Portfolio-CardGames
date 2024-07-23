using System.Diagnostics.CodeAnalysis;

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

	public static bool operator ==(RGBColour _this, RGBColour _other)
	{
		return _this.R == _other.R && _this.G == _other.G && _this.B == _other.B;
	}
	public static bool operator !=(RGBColour _this, RGBColour _other)
	{
		return !(_this == _other);
	}

	public override string ToString()
	{
		return string.Format("R: {0}, G: {1}, B: {2}", R, G, B);
	}
}
