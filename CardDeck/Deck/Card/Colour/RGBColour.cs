using System.Diagnostics.CodeAnalysis;

namespace Deck.Deck.Card.Colour;

public readonly struct RGBColour
{
	public static readonly RGBColour Default = new RGBColour();

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

		hash = HashCode.Combine(Name, R, G, B, HasValue);
	}
	public RGBColour()
	{
		Name = "";
		R = default;
		G = default;
		B = default;
		HasValue = false;

		hash = HashCode.Combine(Name, R, G, B, HasValue);
	}

	private readonly int hash;

	public string Name { get; }
	public bool HasValue { get; } = true;
	public byte R { get; }
	public byte G { get; }
	public byte B { get; }

	public static RGBColour GetClassicColour(CardColour _colour) => _colour switch
	{
		CardColour.Yellow => Yellow,
		CardColour.Blue => Blue,
		CardColour.Red => Red,
		CardColour.Green => Green,
		_ => throw new ArgumentOutOfRangeException(nameof(_colour))
	};

	public override string ToString()
	{
		return string.Format("R: {0}, G: {1}, B: {2}", R, G, B);
	}

	public override bool Equals([NotNullWhen(true)] object? _obj)
	{
		var _colour = _obj as RGBColour?;
		if(!_colour.HasValue)
		{
			return false;
		}

		return GetHashCode() == _colour.GetHashCode();
	}

	public override int GetHashCode()
	{
		return hash;
	}

	public static bool operator ==(RGBColour _left, RGBColour _right)
	{
		return _left.Equals(_right);
	}

	public static bool operator !=(RGBColour _left, RGBColour _right)
	{
		return !(_left == _right);
	}
}
