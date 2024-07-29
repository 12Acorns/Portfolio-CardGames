using System.Diagnostics.CodeAnalysis;

namespace Deck.Deck.Card.Colour;

public readonly struct RGBColour : IColour
{
	public static readonly RGBColour Default = new();

	public static readonly RGBColour White = new("White", 255, 255, 255);
	public static readonly RGBColour Black = new("Black", 0, 0, 0);
	public static readonly RGBColour Red = new("Red", 255, 0, 0);
	public static readonly RGBColour Green = new("Green", 0, 255, 0);
	public static readonly RGBColour Blue = new("Blue", 0, 0, 255);
	public static readonly RGBColour Yellow = new("Yellow", 255, 0, 255);

	public RGBColour(string _name, byte _r, byte _g, byte _b)
	{
		Name = _name;
		R = _r;
		G = _g;
		B = _b;
	}
	public RGBColour()
	{
		Name = "";
		HasValue = false;
	}

	public string Name { get; }
	public byte R { get; }
	public byte G { get; }
	public byte B { get; }
	public bool HasValue { get; } = true;

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
