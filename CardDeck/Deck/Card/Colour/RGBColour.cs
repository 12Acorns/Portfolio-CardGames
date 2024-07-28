using System.Diagnostics.CodeAnalysis;

namespace Deck.Deck.Card.Colour;

public readonly struct RGBColour : IColourSet
{
	public static readonly RGBColour White = new(255, 255, 255);
	public static readonly RGBColour Black = new(0, 0, 0);
	public static readonly RGBColour Red = new(255, 0, 0);
	public static readonly RGBColour Green = new(0, 255, 0);
	public static readonly RGBColour Blue = new(0, 0, 255);
	public static readonly RGBColour Yellow = new(255, 0, 255);

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
		yield return this;
	}

	public static RGBColour GetClassicColour(CardColour _colour) => _colour switch
	{
		CardColour.Yellow => Yellow,
		CardColour.Blue => Blue,
		CardColour.Red => Red,
		CardColour.Green => Green,
		_ => throw new ArgumentOutOfRangeException(nameof(_colour))
	};

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

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if(obj == null)
		{
			return false;
		}
		if(obj is not RGBColour _colour)
		{
			return false;
		}
		return _colour == this;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(R, G, B);
	}
}
